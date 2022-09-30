using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Services;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.Common.ASChildModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Wpf.MvvmLight.SelfHost.ViewModel.ASChildViewModel
{
  public class GripperLoadViewModel : ViewModelBase, IEventBus
  {
    private GripperLoadInfoServices gripperLoadInfoServices;
    private ConfigServices configServices;
    private GripperLoadModel gripperLoadModel;

    public GripperLoadModel GripperLoadModel { get => gripperLoadModel; set { gripperLoadModel = value; RaisePropertyChanged(); } }

    public GripperLoadViewModel()
    {
      gripperLoadInfoServices = new GripperLoadInfoServices();
      configServices = new ConfigServices();
      GripperLoadModel = new GripperLoadModel();

      RegisterMessageSignal();

      EventSignal.SendShowProgressViewSignal(true);
      Task.Run(async () =>
      {
        await InitGripperLoadModel();
        EventSignal.SendShowProgressViewSignal(false);
      });
    }

    /// <summary>
    /// 初始化机型设置模型
    /// </summary>
    private async Task InitGripperLoadModel()
    {
      var moduleTypeConfig = configServices.GetSettings(c => c.Key == "ModuleType").Result?.FirstOrDefault();
      var currentModuleType = moduleTypeConfig != null ? moduleTypeConfig.Value : string.Empty;
      var defaultGripperLoadList = new ObservableCollection<GripperLoadInfo>(
        Enumerable.Range(1, 4).Select(
          i => new GripperLoadInfo
          {
            GripperNumber = i,
            X = 0,
            Y = 0,
            Z = 0,
            RZ = 0,
            ModuleType = currentModuleType,
          })
        );
      var gripperLoadList = new ObservableCollection<GripperLoadInfo>(await gripperLoadInfoServices.GetSettings(g => g.ModuleType == currentModuleType));
      GripperLoadModel.GripperLoadList = gripperLoadList.Count == 0 ? defaultGripperLoadList : gripperLoadList;
    }

    public void RegisterMessageSignal()
    {
      MessengerInstance.Register<string>(this, "SaveAdvancedSettingsSignal", SaveAdvancedSettingsSlot);
      MessengerInstance.Register<string>(this, "UpdateViewByModuleTypeSignal", UpdateViewByModuleTypeSlot);
    }

    private void UpdateViewByModuleTypeSlot(string obj)
    {
      Task.Run(async () =>
      {
        await InitGripperLoadModel();
      });
    }

    private void SaveAdvancedSettingsSlot(string obj)
    {
      var gripperLoadList = new List<GripperLoadInfo>(GripperLoadModel.GripperLoadList);
      gripperLoadInfoServices.SaveSettings(gripperLoadList, g => new { g.Id, g.GripperNumber, g.ModuleType }).Wait();
      UpdateViewByModuleTypeSlot(null);
      MessengerInstance.Send("抓手负载定位信息保存成功!", "ShowMessageSignal");
    }

    public void UnregisterMessageSignal()
    {
      throw new System.NotImplementedException();
    }
  }
}
