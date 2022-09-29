using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Common.Model;
using System.Collections.ObjectModel;

namespace Wpf.MvvmLight.SelfHost.Common
{
  public class LayoutModel : ObservableObject
  {

    private object contentObj;
    private bool isLeftDrawerOpen;
    private ObservableCollection<MenuBar> menuBars;
    private MenuBar activeMenuBar;
    private int leftDrawerWidth;
    private int headerIconSize;
    private MenuBar bottomMenuBar;

    /// <summary>
    /// 是否显示左侧抽屉菜单
    /// </summary>
    public bool IsLeftDrawerOpen
    {
      get { return isLeftDrawerOpen; }
      set { isLeftDrawerOpen = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 视图展示区
    /// </summary>
    public object ContentObj { get => contentObj; set { contentObj = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 菜单字典
    /// </summary>
    public ObservableCollection<MenuBar> MenuBars { get => menuBars; set { menuBars = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 激活的菜单项
    /// </summary>
    public MenuBar ActiveMenuBar { get => activeMenuBar; set { activeMenuBar = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 左侧菜单抽屉宽度
    /// </summary>
    public int LeftDrawerWidth { get => leftDrawerWidth; set { leftDrawerWidth = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 左侧菜单底部操作栏
    /// new MenuBar() { Icon = "Forwardburger", Title = "展开" }
    /// new MenuBar() { Icon = "Backburger", Title = "返回" })
    /// </summary>
    public MenuBar BottomMenuBar { get => bottomMenuBar; set { bottomMenuBar = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 左侧菜单头部图标大小
    /// </summary>
    public int HeaderIconSize { get => headerIconSize; set { headerIconSize = value; RaisePropertyChanged(); } }
  }
}
