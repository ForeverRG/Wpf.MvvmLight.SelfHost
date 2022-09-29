using System.Collections.Concurrent;

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
  }
}

