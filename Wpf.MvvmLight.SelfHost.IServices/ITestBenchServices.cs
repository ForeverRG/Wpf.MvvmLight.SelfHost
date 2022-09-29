using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface ITestBenchServices : IBaseServices<TestBench>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<IList<TestBench>> GetSettings();

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<IList<TestBench>> GetSettings(Expression<Func<TestBench, bool>> expression);

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(IList<TestBench> testBenches);

    Task UpdateRunning(TestBench testBench);

    Task<int> GetTestSequenceCountInUsed();

    /// <summary>
    /// 测试台是否被启用
    /// </summary>
    /// <param name="testBenchNumber"></param>
    /// <returns></returns>
    Task<bool> IsTestBenchUsed(int testBenchNumber);
  }
}
