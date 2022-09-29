using Wpf.MvvmLight.SelfHost.Common.Model;
using Wpf.MvvmLight.SelfHost.EventBus;

namespace Wpf.MvvmLight.SelfHost.Common.Services
{
  /// <summary>
  /// 任务处理服务
  /// </summary>
  public class HandleRunTaskServices
  {
    public HandleRunTaskServices()
    {
    }
    /// <summary>
    /// 处理从夹具取的任务
    /// </summary>
    /// <param name="task"></param>
    public void HandleSingleRunTask(RunTask task)
    {
      var tasks = task.CommonTaskQue;
      _ = tasks.TryDequeue(out CommonTask commonTask);
      var taskContent = commonTask.Content;
      EventSignal.SendWriteLogSignal($"任务类型:{task.TaskFlag},任务内容:{taskContent}");
    }

    /// <summary>
    /// 处理放入夹具的任务
    /// </summary>
    /// <param name="task"></param>
    /// <param name="baseHelper"></param>
    public void HandleMultiRunTask(RunTask task)
    {
      var tasks = task.CommonTaskQue;
      _ = tasks.TryDequeue(out CommonTask commonTask1);
      _ = tasks.TryDequeue(out CommonTask commonTask2);
      var taskContent1 = commonTask1.Content;
      var taskContent2 = commonTask2.Content;
      EventSignal.SendWriteLogSignal($"任务类型:{task.TaskFlag},任务内容:1.{taskContent1};2.{taskContent2}");
    }
  }
}

