using GalaSoft.MvvmLight;

namespace Wpf.MvvmLight.SelfHost.Common.Model
{
  /// <summary>
  /// 系统导航菜单实体类
  /// </summary>
  public class MenuBar : ObservableObject
  {
    private string icon;

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string Icon
    {
      get { return icon; }
      set { icon = value; }
    }

    private string title;

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string Title
    {
      get { return title; }
      set { title = value; }
    }

    private string className;

    /// <summary>
    /// 菜单类名
    /// </summary>
    public string ClassName
    {
      get { return className; }
      set { className = value; }
    }
  }
}
