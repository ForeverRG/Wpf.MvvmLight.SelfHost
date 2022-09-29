using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Model
{
  public class ModuleType
  {
    /// <summary>
    /// id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 机型名称
    /// </summary>
    public string Name { get; set; }
  }
}
