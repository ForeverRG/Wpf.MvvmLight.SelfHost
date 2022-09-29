using SqlSugar;
using System.Collections.Generic;

namespace Wpf.MvvmLight.SelfHost.Model
{
  /// <summary>
  /// 测试台
  /// </summary>
  [SugarTable("TestBench")]
  public class TestBench : BaseEntity
  {
    /// <summary>
    /// 测试台号
    /// </summary>
    public int Number { get; set; }
    /// <summary>
    /// 工序
    /// </summary>
    public string Sequence { get; set; }
    /// <summary>
    /// 工位名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 是否使用
    /// </summary>
    public bool IsUsed { get; set; }
  }

  public class TestBenchWithJig
  {
    public TestBench TestBench { get; set; }
    public Jig Jig { get; set; }
    public List<Jig> Jigs { get; set; }
  }

  public class TestBenchWithSocket
  {
    public TestBench TestBench { get; set; }
    public string Socket { get; set; }
  }
}
