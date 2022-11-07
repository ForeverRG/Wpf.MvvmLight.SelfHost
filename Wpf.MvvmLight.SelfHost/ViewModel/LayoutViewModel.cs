using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using Wpf.MvvmLight.SelfHost.Common;
using Wpf.MvvmLight.SelfHost.Common.Model;
using Wpf.MvvmLight.SelfHost.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wpf.MvvmLight.SelfHost.EventBus;

namespace Wpf.MvvmLight.SelfHost.ViewModel
{
  public class LayoutViewModel : ViewModelBase
  {
    private LayoutModel _layout;
    private Dictionary<Type, object> _cachViews; // 视图缓存

    public RelayCommand<MenuBar> ShowViewCommand { get; set; }
    public RelayCommand ShowDrawerCommand { get; set; }
    public RelayCommand DrawerBackOrForwardCommand { get; set; }
    public RelayCommand ConfigureVisionCommand { get; set; }
    public LayoutModel Layout { get => _layout; set { _layout = value; RaisePropertyChanged(); } }
    public Dictionary<Type, object> CachViews { get => _cachViews; set { _cachViews = value; RaisePropertyChanged(); } }

    public LayoutViewModel()
    {
      DispatcherHelper.Initialize();

      Layout = new LayoutModel()
      {
        MenuBars = CreateMenuBar(),
        ContentObj = new HomeView(),
        IsLeftDrawerOpen = true,
        LeftDrawerWidth = 50,
        HeaderIconSize = 30,
        BottomMenuBar = new MenuBar { Icon = "Forwardburger", Title = "展开" }
      };
      Layout.ActiveMenuBar = Layout.MenuBars.FirstOrDefault();
      CachViews = new Dictionary<Type, object>();
      CachViews.Add(typeof(HomeView), Layout.ContentObj);

      ShowViewCommand = new RelayCommand<MenuBar>(ShowView);
      ShowDrawerCommand = new RelayCommand(ShowDrawer);
      DrawerBackOrForwardCommand = new RelayCommand(DrawerBackOrForward);
      ConfigureVisionCommand = new RelayCommand(ConfigureVision);
    }

    /// <summary>
    /// 配置视觉参数
    /// </summary>
    private void ConfigureVision()
    {
      EventSignal.SendWriteLogSignal("模块开发中...");
    }

    /// <summary>
    /// 收缩或展开抽屉菜单
    /// </summary>
    private void DrawerBackOrForward()
    {
      if (Layout.LeftDrawerWidth == 50)
      {
        Layout.LeftDrawerWidth = 220;
        Layout.HeaderIconSize = 70;
        Layout.BottomMenuBar = new MenuBar { Icon = "Backburger", Title = "收回菜单" };
      }
      else
      {
        Layout.LeftDrawerWidth = 50;
        Layout.HeaderIconSize = 30;
        Layout.BottomMenuBar = new MenuBar { Icon = "Forwardburger", Title = "展开菜单" };
      }
    }

    /// <summary>
    /// 显示抽屉菜单
    /// </summary>
    private void ShowDrawer()
    {
      Layout.IsLeftDrawerOpen = true;
    }

    /// <summary>
    /// 显示view
    /// </summary>
    /// <param name="menuBar"></param>
    private void ShowView(MenuBar menuBar)
    {
      if (string.IsNullOrEmpty(menuBar.ClassName))
      {
        Layout.IsLeftDrawerOpen = false;
        return;
      }
      Layout.ActiveMenuBar = menuBar;
      Type type = Type.GetType($"Wpf.MvvmLight.SelfHost.View.{menuBar.ClassName}");
      object viewObj;
      if (!CachViews.ContainsKey(type))
      {
        viewObj = type.Assembly.CreateInstance(type.FullName);
        CachViews.Add(type, viewObj);
      }
      else
      {
        viewObj = CachViews[type];
      }
      Layout.ContentObj = viewObj;
    }

    /// <summary>
    /// 生成菜单项
    /// </summary>
    /// <returns></returns>
    private ObservableCollection<MenuBar> CreateMenuBar()
    {
      var menuBars = new ObservableCollection<MenuBar>();
      menuBars.Add(new MenuBar() { Icon = "Home", Title = "主页", ClassName = "HomeView" });
      menuBars.Add(new MenuBar() { Icon = "CogOutline", Title = "常规设置", ClassName = "SettingsView" });
      menuBars.Add(new MenuBar() { Icon = "FileCogOutline", Title = "高级设置", ClassName = "AdvancedSettingsView" });
      menuBars.Add(new MenuBar() { Icon = "Console", Title = "运行调试", ClassName = "RunAssistantView" });
      menuBars.Add(new MenuBar() { Icon = "Lan", Title = "TCP调试", ClassName = "TCPAssistantView" });
      menuBars.Add(new MenuBar() { Icon = "ConsoleNetworkOutline", Title = "PLC调试", ClassName = "PLCAssistantView" });
      return menuBars;
    }
  }
}
