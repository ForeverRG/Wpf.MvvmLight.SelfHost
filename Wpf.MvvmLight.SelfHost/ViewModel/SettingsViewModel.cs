using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Services;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.ViewModel
{
  public class SettingsViewModel : ViewModelBase
  {
    private ControlCenterService controlCenterService;
    private PLCServices plcServices;
    private RobotServices robotServices;
    private VisionServices visionServices;
    private TestBenchServices testBenchServices;
    private ConfigServices configServices;
    private ModuleTypeServices moduleTypeServices;

    private SettingsModel settings;

    public RelayCommand SaveAllSettingsCommand { get; private set; }

    /// <summary>
    /// 工位
    /// </summary>
    public IList<string> Station => new List<string> { "DL_FW", "RF_CAL", "RF_FT", "FCT", "ALL_PIN", "RDSIG" };

    /// <summary>
    /// 工序
    /// </summary>
    public IList<string> WorkingProcedure => new List<string> { "1", "2" };

    /// <summary>
    /// 运行模式
    /// </summary>
    public IList<string> RunMode => new List<string> { "A", "AB", "AA" };

    /// <summary>
    /// 条光使能
    /// </summary>
    public IList<string> BarLightEnable => new List<string> { "前后", "左右", "所有", "关闭" };

    public SettingsModel Settings { get => settings; set { settings = value; RaisePropertyChanged(); } }

    public SettingsViewModel()
    {
      controlCenterService = new ControlCenterService();
      plcServices = new PLCServices();
      robotServices = new RobotServices();
      visionServices = new VisionServices();
      testBenchServices = new TestBenchServices();
      configServices = new ConfigServices();
      Settings = new SettingsModel();
      moduleTypeServices = new ModuleTypeServices();

      SaveAllSettingsCommand = new RelayCommand(SaveAllSettings);

      EventSignal.SendShowProgressViewSignal(true);
      // 初始化基本设置
      Task.Run(async () =>
      {
        await InitSettingsModel();
        EventSignal.SendShowProgressViewSignal(false);
      });
    }

    /// <summary>
    /// 保存所有配置
    /// </summary>
    private void SaveAllSettings()
    {
      try
      {
        // 保存总控、PLC、视觉、机器人网络设置
        controlCenterService.SaveSettings(Settings.ControlCenter).Wait();
        plcServices.SaveSettings(Settings.Plc).Wait();
        visionServices.SaveSettings(Settings.Vision).Wait();
        robotServices.SaveSettings(Settings.Robot).Wait();

        // 保存运行参数设置
        configServices.SaveSettings(new List<Config> {
          Settings.ModeConfig,
          Settings.RobotNameConfig,
          Settings.LineNameConfig,
          Settings.OperatorConfig,
          Settings.MoConfig,
          Settings.ModuleTypeConfig,
          Settings.TestModuleNumberConfig,
          Settings.BarLightEnableConfig,
          Settings.RunStartIndexConfig
        }).Wait();

        // 保存测试台设置
        testBenchServices.SaveSettings(new List<TestBench>(Settings.TestBenches)).Wait();

        Messenger.Default.Send("保存成功!", "ShowMessageSignal");
        Messenger.Default.Send(string.Empty, "UpdateViewByModuleTypeSignal");
        Messenger.Default.Send(string.Empty, "UpdateAdvancedSettingsInfoSignal");
      }
      catch (Exception e)
      {
        Messenger.Default.Send(e.Message, "ShowMessageSignal");
        throw;
      }
    }

    /// <summary>
    /// 初始化基本设置
    /// </summary>
    private async Task InitSettingsModel()
    {
      // 初始化总控、PLC、视觉、机器人网络设置
      Settings.ControlCenter = (await controlCenterService.GetSettings()) ?? new ControlCenter { Ip = controlCenterService.GetLocalIpAddress(), Port = 5000 };
      Settings.Plc = (await plcServices.GetSettings()) ?? new PLC { Ip = "192.168.3.16", Port = 6000 };
      Settings.Vision = (await visionServices.GetSettings()) ?? new VisionModel { Ip = "192.168.3.100", Port = 6000 };
      Settings.Robot = (await robotServices.GetSettings()) ?? new Robot { Ip = "192.168.3.10", Port = 6000 };

      //初始化运行参数设置
      var configs = await configServices.GetSettings() as List<Config>;
      Settings.ModeConfig = configs.Find(c => c.Key == "RunMode") ?? new Config { Key = "RunMode", Value = string.Empty };
      Settings.RobotNameConfig = configs.Find(c => c.Key == "RobotName") ?? new Config { Key = "RobotName", Value = string.Empty };
      Settings.LineNameConfig = configs.Find(c => c.Key == "LineName") ?? new Config { Key = "LineName", Value = string.Empty };
      Settings.OperatorConfig = configs.Find(c => c.Key == "Operator") ?? new Config { Key = "Operator", Value = string.Empty };
      Settings.MoConfig = configs.Find(c => c.Key == "MO") ?? new Config { Key = "MO", Value = string.Empty };
      Settings.ModuleTypeConfig = configs.Find(c => c.Key == "ModuleType") ?? new Config { Key = "ModuleType", Value = string.Empty };
      Settings.TestModuleNumberConfig = configs.Find(c => c.Key == "TestModuleNumber") ?? new Config { Key = "TestModuleNumber", Value = string.Empty };
      Settings.BarLightEnableConfig = configs.Find(c => c.Key == "BarLightEnable") ?? new Config { Key = "BarLightEnable", Value = string.Empty };
      Settings.RunStartIndexConfig = configs.Find(c => c.Key == "RunStartIndex") ?? new Config { Key = "RunStartIndex", Value = string.Empty };

      // 初始化测试台设置
      var defaultTestBenches =
          new ObservableCollection<TestBench>(Enumerable.Range(1, 16).Select(
            i => new TestBench
            {
              Number = i,
              Ip = string.Empty,
              IsUsed = false,
              Name = string.Empty,
              Sequence = string.Empty,
              Port = 0
            }));
      var testBenches = new ObservableCollection<TestBench>(await testBenchServices.GetSettings());
      Settings.TestBenches = testBenches.Count == 0 ? defaultTestBenches : testBenches;

      // 初始化其他所需数据
      var moduleTypes = (await moduleTypeServices.GetSettings())?.Select(m => m.Name).ToList();

      Settings.ModuleTypes = moduleTypes.Count > 0 ? moduleTypes : new List<string> { "Module-A", "Module-B" };
    }
  }
}
