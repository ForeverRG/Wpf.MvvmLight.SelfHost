using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Model
{
  public class ModuleSettings
  {
    /// <summary>
    /// id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 机型
    /// </summary>
    public string ModuleType { get; set; }

    /// <summary>
    /// 夹具号
    /// </summary>
    public int JigNumber { get; set; }

    /// <summary>
    /// 使用端口：Socket1/Socket2/DoubleSocket
    /// </summary>
    public string Side { get; set; }
    /// <summary>
    /// 夹具二维码
    /// </summary>
    public string QR { get; set; }
  }
}
