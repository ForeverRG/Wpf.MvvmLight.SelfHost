using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class TestBenchServices : BaseServices<TestBench>, ITestBenchServices
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<IList<TestBench>> GetSettings()
    {
      return await TestBenchDb.Query();
    }

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<IList<TestBench>> GetSettings(Expression<Func<TestBench, bool>> expression)
    {
      return (await TestBenchDb.Query(expression)).ToList();
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    public async Task SaveSettings(IList<TestBench> testBenches)
    {
      var settings = await GetSettings();
      if (settings.Count == 0)
      {
        await TestBenchDb.Add(testBenches.ToList());
      }
      else
      {
        await TestBenchDb.Update(testBenches);
      }
    }

    /// <summary>
    /// 更新测试台使用状态
    /// </summary>
    /// <param name="testBench"></param>
    /// <returns></returns>
    public async Task UpdateRunning(TestBench testBench)
    {
      await TestBenchDb.Update(testBench, new List<string> { "IsRunning" });
    }

    /// <summary>
    /// 获取总共已启用的序数(去重)
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetTestSequenceCountInUsed()
    {
      var sequence = (await TestBenchDb.Query(tb => tb.IsUsed)).Select(tb => tb.Sequence);
      return sequence.Distinct().Count();
    }

    /// <summary>
    /// 测试台是否被启用
    /// </summary>
    /// <param name="testBenchNumber"></param>
    /// <returns></returns>
    public async Task<bool> IsTestBenchUsed(int testBenchNumber)
    {
      var testBenchList = await GetSettings(t => t.Number == testBenchNumber);
      var testBench = testBenchList.FirstOrDefault();
      if (testBench.IsUsed)
      {
        return true;
      }
      return false;
    }
  }
}
