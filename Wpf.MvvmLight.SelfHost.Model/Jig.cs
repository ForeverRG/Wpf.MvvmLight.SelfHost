using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Model
{
  public class Jig
  {
    /// <summary>
    /// id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 夹具号
    /// </summary>
    public int Number { get; set; }

    #region 坐标值
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double RZ { get; set; }
    #endregion

    #region 偏移值
    public double OffsetX { get; set; }
    public double OffsetY { get; set; }
    public double OffsetZ { get; set; }
    public double OffsetRZ { get; set; }
    #endregion

    #region 负载偏移值
    public double LoadOffsetX { get; set; }
    public double LoadOffsetY { get; set; }
    public double LoadOffsetZ { get; set; }
    public double LoadOffsetRZ { get; set; }
    #endregion

    /// <summary>
    /// 类型：QR/Socket1/Socket2
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 夹具二维码
    /// </summary>
    public string QR { get; set; }
    /// <summary>
    /// 对应机型
    /// </summary>
    public string ModuleType { get; set; }
    /// <summary>
    /// 是否使用
    /// </summary>
    public bool IsUsed { get; set; }
  }
}
