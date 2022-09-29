using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;

namespace Wpf.MvvmLight.SelfHost.Common.ASChildModel
{
  public class TrayModel : ObservableObject
  {
    private Tray leftTestTray;
    private Tray rightTestTray;
    private Tray ngTrayLeft;
    private Tray ngTrayRight;
    private Tray goldTray;
    private Tray trayLeftAndRightSpacing;

    /// <summary>
    /// 左侧待测盘
    /// </summary>
    public Tray LeftTestTray
    {
      get { return leftTestTray; }
      set { leftTestTray = value; }
    }

    /// <summary>
    /// 左侧ng盘
    /// </summary>
    public Tray NgTrayLeft { get => ngTrayLeft; set { ngTrayLeft = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 右侧待测盘
    /// </summary>
    public Tray RightTestTray { get => rightTestTray; set { rightTestTray = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 右侧ng盘
    /// </summary>
    public Tray NgTrayRight { get => ngTrayRight; set { ngTrayRight = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 金板盘
    /// </summary>
    public Tray GoldTray { get => goldTray; set { goldTray = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 左右待测盘间距
    /// </summary>
    public Tray TrayLeftAndRightSpacing { get => trayLeftAndRightSpacing; set { trayLeftAndRightSpacing = value; RaisePropertyChanged(); } }
  }
}
