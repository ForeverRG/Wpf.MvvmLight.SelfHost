using Wpf.MvvmLight.SelfHost.Common.Model;
using Wpf.MvvmLight.SelfHost.Common.Services;
using Wpf.MvvmLight.SelfHost.EventBus;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Wpf.MvvmLight.SelfHost.Common.Helpers
{
  /// <summary>
  /// 总控操作帮助类
  /// </summary>
  public class ControlCenterHelper
  {
    public ControlCenterRunningInstance ControlCenterRunningInstance { get; private set; }
    public RunTaskEnqueueManager RunTaskEnqueueManager { get; private set; }
    public bool IsCancelHandlerTask { get; private set; } // 是否取消处理者任务
    private readonly HandleRunTaskServicesDispatcher handleRunTaskDispatcher;
    private readonly ManualResetEvent handlerTaskManualReset;

    public ControlCenterHelper()
    {
      handleRunTaskDispatcher = new HandleRunTaskServicesDispatcher();
      InitControlCenterRunningInstance();
      handlerTaskManualReset = new ManualResetEvent(true);
      IsCancelHandlerTask = false;
    }

    /// <summary>
    /// 初始化总控运行中实例
    /// </summary>
    public void InitControlCenterRunningInstance()
    {
      ControlCenterRunningInstance = new ControlCenterRunningInstance();
      ControlCenterRunningInstance.Text = "Hello ControlCenterHelper!";
      ControlCenterRunningInstance.RunTaskQue = new ConcurrentQueue<RunTask>(); // 初始化任务队列
      RunTaskEnqueueManager = new RunTaskEnqueueManager(ControlCenterRunningInstance);
    }

    #region 从任务队列中取任务的相关操作

    /// <summary>
    /// 从任务队列中获取任务
    /// </summary>
    /// <returns></returns>
    public Task CreateHandlerTask()
    {
      return Task.Run(() =>
      {
        EventSignal.SendWriteLogSignal($"等待执行任务...");
        while (true)
        {
          if (IsCancelHandlerTask)
          {
            return;
          }
          HandleFollowTasks();
          Task.Delay(500).Wait();
        }
      });
    }

    /// <summary>
    /// 处理队列任务
    /// </summary>
    private void HandleFollowTasks()
    {
      var flag = ControlCenterRunningInstance.RunTaskQue.TryDequeue(out RunTask task);
      if (flag)
      {
        handleRunTaskDispatcher.HandleRunTask(task.TaskFlag, task);
      }
    }

    /// <summary>
    /// 停止从队列中取任务
    /// </summary>
    public void StopRunTasksDequeueTask()
    {
      IsCancelHandlerTask = true;
    }

    /// <summary>
    /// 开始从队列中取任务
    /// </summary>
    public void StartRunTasksDequeueTask()
    {
      IsCancelHandlerTask = false;
    }

    /// <summary>
    /// 暂停从队列中取任务
    /// </summary>
    public void SuspendRunTasksDequeueTask()
    {
      handlerTaskManualReset.Reset();
    }

    /// <summary>
    /// 继续从队列中取任务
    /// </summary>
    public void ContinueRunTasksDequeueTask()
    {
      handlerTaskManualReset.Set();
    }

    #endregion
  }
}

