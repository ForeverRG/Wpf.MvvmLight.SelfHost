using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Model
{
  public class BaseEntity
  {
    /// <summary>
    /// id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    /// <summary>
    /// 运行状态
    /// </summary>
    public bool IsRunning { get; set; } = false;
    /// <summary>
    /// ip地址
    /// </summary>
    public string Ip { get; set; }
    /// <summary>
    /// 监听端口号
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// 行为
    /// </summary>
    //public string Action { get; set; }
  }
}
