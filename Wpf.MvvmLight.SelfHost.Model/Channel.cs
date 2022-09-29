using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Model
{
  public class Channel
  {
    /// <summary>
    /// id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    /// <summary>
    /// 通道号
    /// </summary>
    public int Number { get; set; }
    /// <summary>
    /// 测试台号
    /// </summary>
    public int TestBenchNumber { get; set; }
    /// <summary>
    /// 夹具socket
    /// </summary>
    public string Socket { get; set; }
  }
}
