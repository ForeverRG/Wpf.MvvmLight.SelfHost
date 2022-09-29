namespace Wpf.MvvmLight.SelfHost.Model.GlobalVariable
{
  /// <summary>
  /// 机器人执行操作后的返回值
  /// </summary>
  public class RobotReturn
  {
    /// <summary>
    /// 机器人返回HOME点
    /// </summary>
    public const string ROBOTOK_60 = "ROBOTOK,60";
    /// <summary>
    /// 机器人去拍照定位/读码
    /// </summary>
    public const string ROBOTOK_61 = "ROBOTOK,61";
    /// <summary>
    /// 到入料盘/金板区/夹具/PASS盘/NG盘取放产品
    /// </summary>
    public const string ROBOTOK_62 = "ROBOTOK,62";
    /// <summary>
    /// 断开连接，返回home
    /// </summary>
    public const string ROBOTOK_404 = "ROBOTOK,404";
    /// <summary>
    /// 气缸异常
    /// </summary>
    public const string CylinderFAIL = "CylinderFAIL";
    /// <summary>
    /// 吸嘴异常
    /// </summary>
    public const string VacuumFAIL = "VacuumFAIL";
    /// <summary>
    /// 总控命名异常
    /// </summary>
    public const string COMMANDFAIL = "COMMANDFAIL";
  }
}
