using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IJigServices : IBaseServices<Jig>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<IList<Jig>> GetSettings();

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(IList<Jig> jigs);
  }
}
