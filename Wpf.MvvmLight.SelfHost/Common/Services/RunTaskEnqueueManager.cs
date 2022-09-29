using Wpf.MvvmLight.SelfHost.Common.Model;

namespace Wpf.MvvmLight.SelfHost.Common.Services
{
  /// <summary>
  /// 运行任务管家
  /// </summary>
  public class RunTaskEnqueueManager
  {
    private readonly ControlCenterRunningInstance controlCenterRunningInstance;

    public RunTaskEnqueueManager(ControlCenterRunningInstance controlCenterRunningInstance)
    {
      this.controlCenterRunningInstance = controlCenterRunningInstance;
    }

    /// <summary>
    /// 追加单个任务
    /// </summary>
    public void EnqueueSignleRunTask()
    {
      RunTask runTask = new RunTask();
      runTask.TaskFlag = 1;
      runTask.CommonTaskQue.Enqueue(
        new CommonTask
        {
          Content = "单任务"
        });
      controlCenterRunningInstance.RunTaskQue.Enqueue(runTask);
    }

    /// <summary>
    /// 追加多个任务
    /// </summary>
    public void EnqueueMultiRunTask()
    {
      RunTask runTask = new RunTask();
      runTask.TaskFlag = 2;
      runTask.CommonTaskQue.Enqueue(
        new CommonTask
        {
          Content = "多任务1"
        });
      runTask.CommonTaskQue.Enqueue(
        new CommonTask
        {
          Content = "多任务2"
        });
      controlCenterRunningInstance.RunTaskQue.Enqueue(runTask);
    }
  }
}
