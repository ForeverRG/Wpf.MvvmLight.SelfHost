using Quartz.Impl;
using Quartz;
using System;
using System.Threading.Tasks;
using Wpf.MvvmLight.SelfHost.Tasks.Jobs;

namespace Wpf.MvvmLight.SelfHost.Tasks
{
  public class QuartzTaskScheduler
  {
    public static async Task CreateScheduler()
    {
      var schedulerFactory = new StdSchedulerFactory();
      var scheduler = await schedulerFactory.GetScheduler();
      await scheduler.Start();
      Console.WriteLine($"任务调度器已启动");

      //创建作业和触发器
      var jobDetail = JobBuilder.Create<HelloWorldJob>().Build();
      var trigger = TriggerBuilder.Create()
                                  .WithSimpleSchedule(m =>
                                  {
                                    m.WithRepeatCount(10).WithIntervalInSeconds(3);
                                  })
                                  .Build();

      //添加调度
      await scheduler.ScheduleJob(jobDetail, trigger);
    }
  }
}
