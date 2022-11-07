using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.Common.ASChildModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wpf.MvvmLight.SelfHost.IServices;

namespace Wpf.MvvmLight.SelfHost.ViewModel.ASChildViewModel
{
  public class ModuleSettingsViewModel : ViewModelBase, IEventBus
  {
    private IModuleSettingsServices _moduleSettingsServices;
    private IJigServices _jigServices;
    private IModuleTypeServices _moduleTypeServices;
    private ModuleSettingsModel _moduleSettingsModel;

    public ModuleSettingsModel ModuleSettingsModel { get => _moduleSettingsModel; set { _moduleSettingsModel = value; RaisePropertyChanged(); } }

    public ModuleSettingsViewModel(
      IModuleSettingsServices moduleSettingsServices,
      IJigServices jigServices,
      IModuleTypeServices moduleTypeServices)
    {
      _moduleTypeServices = moduleTypeServices;
      _moduleSettingsServices = moduleSettingsServices;
      _jigServices = jigServices;
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
      ModuleSettingsModel.ModuleTypes = (await _moduleTypeServices.GetSettings())?.Select(m => m.Name).ToList(); ;
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
      var moduleSettings = new ObservableCollection<ModuleSettings>(await _moduleSettingsServices.GetSettings());
      ModuleSettingsModel.ModuleSettings = moduleSettings.Count == 0 ? defaultModuleSettings : moduleSettings;
    }

    public void RegisterMessageSignal()
    {
      MessengerInstance.Register<string>(this, "SaveAdvancedSettingsSignal", SaveAdvancedSettingsSlot);
    }

    private void SaveAdvancedSettingsSlot(string obj)
    {
      _moduleSettingsServices.SaveSettings(new List<ModuleSettings>(ModuleSettingsModel.ModuleSettings)).Wait();
      foreach (var moduleSetting in ModuleSettingsModel.ModuleSettings)
      {
        _jigServices.UpdateJigField(j => new Jig { QR = moduleSetting.QR }, j => j.Number == moduleSetting.JigNumber);
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
