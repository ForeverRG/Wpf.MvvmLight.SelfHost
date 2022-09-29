using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Concurrent;

namespace Wpf.MvvmLight.SelfHost.Common.Model
{
  /// <summary>
  /// 运行任务
  /// </summary>
  public class RunTask
  {
    /// <summary>
    /// 任务类型
    /// </summary>
    public int TaskFlag { get; set; }
    /// <summary>
    /// 任务队列
    /// </summary>
    public ConcurrentQueue<CommonTask> CommonTaskQue { get; set; } = new ConcurrentQueue<CommonTask>();
  }

  /// <summary>
  /// 任务模型
  /// </summary>
  public class CommonTask
  {
    /// <summary>
    /// 任务内容
    /// </summary>
    public string Content { get; set; }
  }
}
