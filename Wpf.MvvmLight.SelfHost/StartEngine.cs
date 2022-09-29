﻿using Wpf.MvvmLight.SelfHost.Common.Helpers;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost
{
  public class StartEngine
  {
    private readonly ControlCenterHelper controlCenterHelper;

    public StartEngine(ControlCenterHelper controlCenterHelper)
    {
      this.controlCenterHelper = controlCenterHelper;
    }

    /// <summary>
    /// 总控启动开始运行任务
    /// </summary>
    public async Task StartRunTask()
    {
      // 重置取消处理者任务信号量
      controlCenterHelper.StartRunTasksDequeueTask();
      // 定时从队列中获取任务
      await controlCenterHelper.CreateHandlerTask();
    }
  }
}
