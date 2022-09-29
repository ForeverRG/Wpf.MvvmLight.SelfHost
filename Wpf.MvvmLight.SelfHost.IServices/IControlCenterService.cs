using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IControlCenterService : IBaseServices<ControlCenter>
  {
    /// <summary>
    /// 获取本机ipv4地址
    /// </summary>
    /// <returns></returns>
    string GetLocalIpAddress();
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<ControlCenter> GetSettings();

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(ControlCenter vision);
  }
}
