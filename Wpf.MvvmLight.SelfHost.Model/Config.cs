using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Model
{
  public class Config
  {
    /// <summary>
    /// id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
  }
}
