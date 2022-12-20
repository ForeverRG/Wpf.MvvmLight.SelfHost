using Wpf.MvvmLight.SelfHost.Common.Model;
using Wpf.MvvmLight.SelfHost.Common.Services;
using Wpf.MvvmLight.SelfHost.EventBus;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Wpf.MvvmLight.SelfHost.Common.Enum;

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

    #region 方式一：从任务队列中取任务的相关操作

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
          handlerTaskManualReset.WaitOne();
          if (IsCancelHandlerTask)
          {
            return;
          }
          HandleFollowTasks();
          Task.Delay(500).Wait();
        }
      });
    }

    #endregion

    #region 方式二：通过定时器实现定时任务

    /// <summary>
    /// 创建Timer1定时处理任务
    /// </summary>
    /// <returns></returns>
    public Timer CreateTimer1Task()
    {
      var cb = new TimerCallback(HandleTimer1FollowTask);
      var timer = new Timer(cb, null, 0, Timeout.Infinite);
      return timer;
    }

    /// <summary>
    /// 处理Timer1任务
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private void HandleTimer1FollowTask(object state)
    {
      handlerTaskManualReset.WaitOne();
      EventSignal.SendWriteLogSignal($"Timer1=>执行Timer1任务1");
      Thread.Sleep(500);
      EventSignal.SendWriteLogSignal($"Timer1=>执行Timer1任务2");
      Thread.Sleep(500);
      HandleFollowTasks();
      // 再次启动入料位定时器
      ContinueRunningTask(RunTimer.Timer1, 0, Timeout.Infinite);
    }

    /// <summary>
    /// 创建Timer2定时处理任务
    /// </summary>
    /// <returns></returns>
    public Timer CreateTimer2Task()
    {
      var cb = new TimerCallback(HandleTimer2FollowTask);
      var timer = new Timer(cb, null, 100, 2000);
      return timer;
    }

    /// <summary>
    /// 处理Timer2任务
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private void HandleTimer2FollowTask(object state)
    {
      handlerTaskManualReset.WaitOne();
      EventSignal.SendWriteLogSignal($"Timer2=>执行Timer2任务1");
      Thread.Sleep(500);
      EventSignal.SendWriteLogSignal($"Timer2=>执行Timer2任务2");
      Thread.Sleep(500);
      HandleFollowTasks();
    }

    #endregion

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

    #region 定时任务操作封装

    /// <summary>
    /// 停止正在运行中的定时任务,释放定时器资源
    /// </summary>
    public void DisposeRunningTask(RunTimer runTimer)
    {
      DisposeTimer(runTimer);
    }

    /// <summary>
    /// 开始正在运行中的定时任务
    /// </summary>
    public void StartRunningTask(RunTimer runTimer, int dueTime = 0, int period = 200)
    {
      ChangeTimer(runTimer, dueTime, period);
    }

    /// <summary>
    /// 暂停正在运行中的定时任务
    /// </summary>
    public void StopRunningTask(RunTimer runTimer)
    {
      ChangeTimer(runTimer, -1, Timeout.Infinite);
    }

    /// <summary>
    /// 继续正在运行中的定时任务
    /// </summary>
    public void ContinueRunningTask(RunTimer runTimer, int dueTime = 0, int period = 200)
    {
      ChangeTimer(runTimer, dueTime, period);
    }

    /// <summary>
    /// 更改计时器的启动时间和方法调用之间的间隔
    /// </summary>
    /// <param name="runTimer">定时器</param>
    /// <param name="dueTime">在调用构造 Timer 时指定的回调方法之前的延迟时间量（以毫秒为单位）</param>
    /// <param name="period">调用构造 Timer 时指定的回调方法的时间间隔（以毫秒为单位）</param>
    private void ChangeTimer(RunTimer runTimer, int dueTime, int period)
    {
      switch (runTimer)
      {
        case RunTimer.All:
          ControlCenterRunningInstance.Timer1?.Change(dueTime, period);
          ControlCenterRunningInstance.Timer2?.Change(dueTime, period);
          break;
        case RunTimer.Timer1:
          ControlCenterRunningInstance.Timer1?.Change(dueTime, period);
          break;
        case RunTimer.Timer2:
          ControlCenterRunningInstance.Timer2?.Change(dueTime, period);
          break;
        default:
          break;
      }
    }

    /// <summary>
    /// 释放由 Timer 的当前实例使用的所有资源
    /// </summary>
    /// <param name="runTimer">定时器</param>
    private void DisposeTimer(RunTimer runTimer)
    {
      switch (runTimer)
      {
        case RunTimer.All:
          ControlCenterRunningInstance.Timer1?.Dispose();
          ControlCenterRunningInstance.Timer2?.Dispose();
          break;
        case RunTimer.Timer1:
          ControlCenterRunningInstance.Timer1?.Dispose();
          break;
        case RunTimer.Timer2:
          ControlCenterRunningInstance.Timer2?.Dispose();
          break;
        default:
          break;
      }
    }
    #endregion
  }
}

