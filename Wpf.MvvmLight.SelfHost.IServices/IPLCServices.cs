using Wpf.MvvmLight.SelfHost.Model;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IPLCServices : IBaseServices<PLC>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<PLC> GetSettings();

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(PLC plc);
  }
}
