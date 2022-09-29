using Quartz;
using System;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.Tasks.Jobs
{
  public class HelloWorldJob : IJob
  {
    public Task Execute(IJobExecutionContext context)
    {
      return Task.Factory.StartNew(() =>
      {
        Console.WriteLine("Hello World");
      });
    }
  }
}
