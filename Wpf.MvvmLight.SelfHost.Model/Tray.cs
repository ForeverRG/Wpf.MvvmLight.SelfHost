using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Model
{
  public class Tray
  {
    /// <summary>
    /// id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    #region 规格
    public int Columns { get; set; }
    public int Rows { get; set; }
    #endregion

    #region 基准点
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double RZ { get; set; }
    #endregion

    #region tray盘间距
    public double SpacingX { get; set; }
    public double SpacingY { get; set; }
    #endregion

    /// <summary>
    /// 类型：待测1盘/待测2盘/NG1盘/NG2盘/金板盘
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 是否使用
    /// </summary>
    public bool IsUsed { get; set; }
  }
}
