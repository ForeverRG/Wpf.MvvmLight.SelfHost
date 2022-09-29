using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Services;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.Common.ASChildModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.ViewModel.ASChildViewModel
{
  public class TrayViewModel : ViewModelBase, IEventBus
  {
    private TrayServices trayServices;
    private TrayModel trayModel;
    public TrayModel TrayModel { get => trayModel; set { trayModel = value; RaisePropertyChanged(); } }

    public TrayViewModel()
    {
      trayServices = new TrayServices();
      TrayModel = new TrayModel();

      RegisterMessageSignal();
      EventSignal.SendShowProgressViewSignal(true);
      Task.Run(async () =>
      {
        await InitTrayModel();
        EventSignal.SendShowProgressViewSignal(false);
      });
    }

    /// <summary>
    /// 初始化tray模型
    /// </summary>
    private async Task InitTrayModel()
    {
      var trays = (await trayServices.GetSettings()).ToList();
      if (trays.Count == 0)
      {
        TrayModel.GoldTray = new Tray { Columns = 0, Rows = 0, X = 0, Y = 0, Z = 0, SpacingX = 0, SpacingY = 0, RZ = 0, Type = "Gold" };
        TrayModel.NgTrayLeft = new Tray { Columns = 0, Rows = 0, X = 0, Y = 0, Z = 0, SpacingX = 0, SpacingY = 0, RZ = 0, Type = "NGLeft" };
        TrayModel.NgTrayRight = new Tray { Columns = 0, Rows = 0, X = 0, Y = 0, Z = 0, SpacingX = 0, SpacingY = 0, RZ = 0, Type = "NGRight" };
        TrayModel.LeftTestTray = new Tray { Columns = 0, Rows = 0, X = 0, Y = 0, Z = 0, SpacingX = 0, SpacingY = 0, RZ = 0, Type = "TestLeft" };
        TrayModel.RightTestTray = new Tray { Columns = 0, Rows = 0, X = 0, Y = 0, Z = 0, SpacingX = 0, SpacingY = 0, RZ = 0, Type = "TestRight" };
        TrayModel.TrayLeftAndRightSpacing = new Tray { Columns = 0, Rows = 0, X = 0, Y = 0, Z = 0, SpacingX = 0, SpacingY = 0, RZ = 0, Type = "LRSpacing" };
      }
      else
      {
        TrayModel.GoldTray = trays.Find(t => t.Type == "Gold");
        TrayModel.NgTrayLeft = trays.Find(t => t.Type == "NGLeft");
        TrayModel.NgTrayRight = trays.Find(t => t.Type == "NGRight");
        TrayModel.LeftTestTray = trays.Find(t => t.Type == "TestLeft");
        TrayModel.RightTestTray = trays.Find(t => t.Type == "TestRight");
        TrayModel.TrayLeftAndRightSpacing = trays.Find(t => t.Type == "LRSpacing");
      }
    }

    public void RegisterMessageSignal()
    {
      MessengerInstance.Register<string>(this, "SaveAdvancedSettingsSignal", SaveAdvancedSettingsSlot);
    }

    private void SaveAdvancedSettingsSlot(string obj)
    {
      trayServices.SaveSettings(new List<Tray> { TrayModel.GoldTray, TrayModel.NgTrayLeft, TrayModel.NgTrayRight, TrayModel.LeftTestTray, TrayModel.RightTestTray, TrayModel.TrayLeftAndRightSpacing }).Wait();
      UpdateTrayView();
      MessengerInstance.Send("Tray盘配置保存成功!", "ShowMessageSignal");
      MessengerInstance.Send(string.Empty, "UpdateTraySettingsSignal");
    }

    /// <summary>
    /// 更新界面
    /// </summary>
    private void UpdateTrayView()
    {
      Task.Run(async () =>
      {
        await InitTrayModel();
      });
    }

    public void UnregisterMessageSignal()
    {
      throw new System.NotImplementedException();
    }
  }
}
