using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Wpf.MvvmLight.SelfHost.Common
{
  public class SettingsModel : ObservableObject
  {
    private PLC plc;
    private ControlCenter controlCenter;
    private Robot robot;
    private VisionModel vision;
    private ObservableCollection<TestBench> testBenches;
    private ObservableCollection<Config> configs;
    private Config modeConfig;
    private Config robotNameConfig;
    private Config lineNameConfig;
    private Config operatorConfig;
    private Config moConfig;
    private Config moduleTypeConfig;
    private Config testModuleNumberConfig;
    private Config barLightEnableConfig;
    private Config runStartIndexConfig;
    private IList<string> moduleTypes;


    /// <summary>
    /// 测试台集合
    /// </summary>
    public ObservableCollection<TestBench> TestBenches
    {
      get => testBenches;
      set { testBenches = value; RaisePropertyChanged(); }
    }
    /// <summary>
    /// 视觉
    /// </summary>
    public VisionModel Vision
    {
      get { return vision; }
      set { vision = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 机器人
    /// </summary>
    public Robot Robot
    {
      get { return robot; }
      set { robot = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 总控
    /// </summary>
    public ControlCenter ControlCenter
    {
      get { return controlCenter; }
      set { controlCenter = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// PLC
    /// </summary>
    public PLC Plc
    {
      get { return plc; }
      set { plc = value; RaisePropertyChanged(); }
    }
    /// <summary>
    /// 所有配置
    /// </summary>
    public ObservableCollection<Config> Configs { get => configs; set { configs = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 模式
    /// </summary>
    public Config ModeConfig { get => modeConfig; set { modeConfig = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 机器人号
    /// </summary>
    public Config RobotNameConfig { get => robotNameConfig; set { robotNameConfig = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 线体号
    /// </summary>
    public Config LineNameConfig { get => lineNameConfig; set { lineNameConfig = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 操作员
    /// </summary>
    public Config OperatorConfig { get => operatorConfig; set { operatorConfig = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 工单号
    /// </summary>
    public Config MoConfig { get => moConfig; set { moConfig = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 机型配置
    /// </summary>
    public Config ModuleTypeConfig { get => moduleTypeConfig; set { moduleTypeConfig = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 待测模块数
    /// </summary>
    public Config TestModuleNumberConfig { get => testModuleNumberConfig; set { testModuleNumberConfig = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 条光使能
    /// </summary>
    public Config BarLightEnableConfig { get => barLightEnableConfig; set { barLightEnableConfig = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 运行的起始模块索引
    /// </summary>
    public Config RunStartIndexConfig { get => runStartIndexConfig; set { runStartIndexConfig = value; RaisePropertyChanged(); } }
    /// <summary>
    /// 机型种类
    /// </summary>
    public IList<string> ModuleTypes { get => moduleTypes; set { moduleTypes = value; RaisePropertyChanged(); } }
  }
}
