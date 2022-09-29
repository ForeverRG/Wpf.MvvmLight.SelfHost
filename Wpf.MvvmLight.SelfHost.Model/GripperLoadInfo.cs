using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Model
{
  public class GripperLoadInfo
  {
    /// <summary>
    /// id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    /// <summary>
    /// 抓手号
    /// </summary>
    public int GripperNumber { get; set; }

    #region 抓手负载坐标
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double RZ { get; set; }
    #endregion

    /// <summary>
    /// 机型
    /// </summary>
    public string ModuleType { get; set; }
  }
}
