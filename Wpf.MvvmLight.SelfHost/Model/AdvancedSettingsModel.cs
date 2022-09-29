using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Common.Model;
using System.Collections.ObjectModel;

namespace Wpf.MvvmLight.SelfHost.Common
{
  public class AdvancedSettingsModel : ObservableObject
  {
    private ObservableCollection<MenuBar> menuBars;
    private object contentObj;
    private string currentModuleType;

    /// <summary>
    /// 菜单项
    /// </summary>
    public ObservableCollection<MenuBar> MenuBars
    {
      get { return menuBars; }
      set { menuBars = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 视图展示区
    /// </summary>
    public object ContentObj { get => contentObj; set { contentObj = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 当前机型
    /// </summary>
    public string CurrentModuleType { get => currentModuleType; set { currentModuleType = value; RaisePropertyChanged(); } }
  }
}
