using Wpf.MvvmLight.SelfHost.Common.Model;

namespace Wpf.MvvmLight.SelfHost.Common.Helpers
{
  /// <summary>
  /// 测试台操作帮助类
  /// </summary>
  public class TestBenchHelper
  {
    public TestBenchRunningInstance TestBenchRunningInstance { get; private set; }

    public TestBenchHelper()
    {
      TestBenchRunningInstance = new TestBenchRunningInstance();
    }
  }
}


