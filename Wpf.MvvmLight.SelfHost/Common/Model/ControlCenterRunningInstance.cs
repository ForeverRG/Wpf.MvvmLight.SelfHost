using System.Collections.Concurrent;
using System.Threading;

namespace Wpf.MvvmLight.SelfHost.Common.Model
{
  /// <summary>
  /// 运行中实例(全局变量)
  /// </summary>
  public class ControlCenterRunningInstance
  {
    public string Text { get; set; }

    /// <summary>
    /// 任务队列
    /// </summary>
    public ConcurrentQueue<RunTask> RunTaskQue { get; set; }
    /// <summary>
    /// 任务定时器1
    /// </summary>
    public Timer Timer1 { get; set; }

    /// <summary>
    /// 任务定时器2
    /// </summary>
    public Timer Timer2 { get; set; }
  }
}

