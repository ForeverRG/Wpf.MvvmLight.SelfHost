using Wpf.MvvmLight.SelfHost.Common.Enum;

namespace Wpf.MvvmLight.SelfHost.Common.Model
{
  public class UILog
  {
    /// <summary>
    /// 日志内容
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// 日志类型
    /// </summary>
    public LogLevel LogLevel { get; set; }
  }
}
