using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Services;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.Common.ASChildModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.ViewModel.ASChildViewModel
{
  public class ModuleSettingsViewModel : ViewModelBase, IEventBus
  {
    private ModuleSettingsServices moduleSettingsServices;
    private JigServices jigServices;
    private ModuleSettingsModel moduleSettingsModel;
    private ModuleTypeServices moduleTypeServices;

    public ModuleSettingsModel ModuleSettingsModel { get => moduleSettingsModel; set { moduleSettingsModel = value; RaisePropertyChanged(); } }

    public ModuleSettingsViewModel()
    {
      moduleTypeServices = new ModuleTypeServices();
      moduleSettingsServices = new ModuleSettingsServices();
      jigServices = new JigServices();
      ModuleSettingsModel = new ModuleSettingsModel();

      RegisterMessageSignal();

      EventSignal.SendShowProgressViewSignal(true);
      Task.Run(async () =>
      {
        await InitModuleSettingsModel();
        EventSignal.SendShowProgressViewSignal(false);
      });
    }

    /// <summary>
    /// 初始化机型设置模型
    /// </summary>
    private async Task InitModuleSettingsModel()
    {
      ModuleSettingsModel.ModuleTypes = (await moduleTypeServices.GetSettings())?.Select(m => m.Name).ToList(); ;
      ModuleSettingsModel.Sides = new List<string> { "Socket1", "Socket2", "DoubleSocket" };

      var defaultModuleSettings = new ObservableCollection<ModuleSettings>(
        Enumerable.Range(1, 16).Select(
          i => new ModuleSettings
          {
            JigNumber = i,
            ModuleType = "BG95",
            QR = "11-11-11",
            Side = "Socket1"
          })
        );
      var moduleSettings = new ObservableCollection<ModuleSettings>(await moduleSettingsServices.GetSettings());
      ModuleSettingsModel.ModuleSettings = moduleSettings.Count == 0 ? defaultModuleSettings : moduleSettings;
    }

    public void RegisterMessageSignal()
    {
      MessengerInstance.Register<string>(this, "SaveAdvancedSettingsSignal", SaveAdvancedSettingsSlot);
    }

    private void SaveAdvancedSettingsSlot(string obj)
    {
      moduleSettingsServices.SaveSettings(new List<ModuleSettings>(ModuleSettingsModel.ModuleSettings)).Wait();
      foreach (var moduleSetting in ModuleSettingsModel.ModuleSettings)
      {
        jigServices.UpdateJigField(j => new Jig { QR = moduleSetting.QR }, j => j.Number == moduleSetting.JigNumber);
      }
      UpdateModuleSettingsView();
      MessengerInstance.Send("机型配置保存成功!", "ShowMessageSignal");
    }

    /// <summary>
    /// 更新界面
    /// </summary>
    private void UpdateModuleSettingsView()
    {
      Task.Run(async () =>
      {
        await InitModuleSettingsModel();
      });
    }

    public void UnregisterMessageSignal()
    {
      throw new System.NotImplementedException();
    }
  }
}
