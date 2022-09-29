using Wpf.MvvmLight.SelfHost.Common;
using Wpf.MvvmLight.SelfHost.Common.Helpers;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.Common.Model;
using System;
using System.Collections.Generic;

namespace Wpf.MvvmLight.SelfHost.Common.Services
{
  /// <summary>
  /// 任务处理服务调度
  /// </summary>
  public class HandleRunTaskServicesDispatcher
  {
    private HandleRunTaskServices services;
    private Dictionary<int, Action<RunTask>> handleRunTaskServicesDic;

    public HandleRunTaskServicesDispatcher()
    {
      services = new HandleRunTaskServices();
      handleRunTaskServicesDic = new Dictionary<int, Action<RunTask>>();
      DispatcherInit();
    }

    /// <summary>
    /// 初始化处理任务字典,添加任务处理服务
    /// </summary>
    private void DispatcherInit()
    {
      handleRunTaskServicesDic.Add(1, (task) => services.HandleSingleRunTask(task));
      handleRunTaskServicesDic.Add(2, (task) => services.HandleMultiRunTask(task));
    }

    /// <summary>
    /// 处理任务
    /// </summary>
    /// <param name="taskFlag"></param>
    /// <param name="task"></param>
    public void HandleRunTask(int taskFlag, RunTask task)
    {
      try
      {
        var flag = handleRunTaskServicesDic.TryGetValue(taskFlag, out Action<RunTask> handleRunTask);
        if (!flag)
        {
          EventSignal.SendWriteAllLogSignal($"未找到对应任务处理者");
          return;
        }
        EventSignal.SendWriteAllLogSignal($"查询到待处理任务类型：{taskFlag}，开始运行任务...");
        handleRunTask.Invoke(task);
      }
      catch (Exception e)
      {
        EventSignal.SendWriteAllLogSignal($"运行任务异常：{e.Message}", Enum.LogLevel.Error);
        throw;
      }
    }
  }
}
