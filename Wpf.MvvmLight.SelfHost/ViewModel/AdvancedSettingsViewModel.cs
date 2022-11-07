using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Wpf.MvvmLight.SelfHost.Common;
using Wpf.MvvmLight.SelfHost.Common.Model;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.View.ASChildView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wpf.MvvmLight.SelfHost.IServices;

namespace Wpf.MvvmLight.SelfHost.ViewModel
{
  public class AdvancedSettingsViewModel : ViewModelBase, IEventBus
  {
    private IConfigServices _configServices;
    private AdvancedSettingsModel _advancedSettings;
    private Dictionary<Type, object> _cachViews; // 视图缓存
    public AdvancedSettingsModel AdvancedSettings { get => _advancedSettings; set { _advancedSettings = value; RaisePropertyChanged(); } }
    public Dictionary<Type, object> CachViews { get => _cachViews; set { _cachViews = value; RaisePropertyChanged(); } }
    public RelayCommand<MenuBar> ShowViewCommand { get; private set; }
    public RelayCommand SaveAllAdvancedSettingsCommand { get; private set; }

    public AdvancedSettingsViewModel(IConfigServices configServices)
    {
      _configServices = configServices;
      ShowViewCommand = new RelayCommand<MenuBar>(ShowView);
      SaveAllAdvancedSettingsCommand = new RelayCommand(SaveAllAdvancedSettings);

      AdvancedSettings = new AdvancedSettingsModel
      {
        MenuBars = CreateMenuBar(),
        ContentObj = new JigView()
      };
      CachViews = new Dictionary<Type, object>();
      CachViews.Add(typeof(JigView), AdvancedSettings.ContentObj);
      RegisterMessageSignal();
      UpdateHeaderInfo();
    }

    #region ViewModel通信
    // 方案一：1.给绑定相应ViewModel的子视图中的相关控件命名；2.父视图ViewModel发送带参命令，参数为子视图对象；3.父视图ViewModel获取子视图对象后，获取已命名的控件对象，该对象包含了想要的数据源。
    // 注：父视图ViewModel带参命令中可通过"object"或者"dynamic"动态解析子视图对象。
    // 该方案只适用于父视图ViewModel获取子视图ViewModel相关数据，不适用子视图ViewModel获取父视图ViewModel数据。较繁琐，且涉嫌数据侵入，不建议使用。
    // 方案二：通过mvvm本身的消息机制来实现两个ViewModel之间的通信，如这里使用mvvmlight中的Messager来实现。此方法无视图关系局限，且无数据侵入，但注意消息的订阅和释放，建议使用。

    #region 方案一
    //private void SaveAllAdvancedSettings(dynamic o)
    //{
    //  var source = o.DataGrid.ItemsSource;
    //}
    #endregion

    #region 方案二
    /// <summary>
    /// 保存所有高级设置
    /// </summary>
    private void SaveAllAdvancedSettings()
    {
      MessengerInstance.Send(string.Empty, "SaveAdvancedSettingsSignal");
    }

    #endregion

    #endregion

    /// <summary>
    /// 导航视图
    /// </summary>
    /// <param name="obj"></param>
    private void ShowView(MenuBar obj)
    {
      Type type = Type.GetType($"Wpf.MvvmLight.SelfHost.View.ASChildView.{obj.ClassName}");
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
      AdvancedSettings.ContentObj = viewObj;
    }

    /// <summary>
    /// 生成菜单项
    /// </summary>
    /// <returns></returns>
    private ObservableCollection<MenuBar> CreateMenuBar()
    {
      var menuBars = new ObservableCollection<MenuBar>();
      menuBars.Add(new MenuBar() { Icon = "ScoreboardOutline", Title = "夹具", ClassName = "JigView" });
      menuBars.Add(new MenuBar() { Icon = "GridLarge", Title = "Tray盘", ClassName = "TrayView" });
      menuBars.Add(new MenuBar() { Icon = "SimOutline", Title = "机型配置", ClassName = "ModuleSettingsView" });
      menuBars.Add(new MenuBar() { Icon = "MapMarkerPath", Title = "通道映射-双A", ClassName = "ChannelMappingView" });
      menuBars.Add(new MenuBar() { Icon = "SelectMarker", Title = "抓手负载定位", ClassName = "GripperLoadView" });
      return menuBars;
    }

    /// <summary>
    /// 更新其他相关信息
    /// </summary>
    private void UpdateHeaderInfo()
    {
      var moduleTypeConfig = _configServices.GetSettings(c => c.Key == "ModuleType").Result?.FirstOrDefault();
      AdvancedSettings.CurrentModuleType = moduleTypeConfig != null ? moduleTypeConfig.Value : string.Empty;
    }

    public void RegisterMessageSignal()
    {
      MessengerInstance.Register<string>(this, "UpdateAdvancedSettingsInfoSignal", UpdateAdvancedSettingsInfoSlot);
    }

    private void UpdateAdvancedSettingsInfoSlot(string msg)
    {
      UpdateHeaderInfo();
    }

    public void UnregisterMessageSignal()
    {
      throw new NotImplementedException();
    }
  }
}
