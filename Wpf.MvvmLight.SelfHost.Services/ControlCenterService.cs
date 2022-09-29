using Wpf.MvvmLight.SelfHost.Common;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Model.Enum;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class ControlCenterService : BaseServices<ControlCenter>, IControlCenterService
  {
    /// <summary>
    /// 获取本地ip
    /// </summary>
    /// <returns></returns>
    public string GetLocalIpAddress()
    {
      return IPAddressUtils.GetLocalIpAddress(NetType.InterNetwork.ToString()).FirstOrDefault();
    }

    /// <summary>
    /// 总控获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<ControlCenter> GetSettings()
    {
      return (await ControlCenterDb.Query()).FirstOrDefault();
    }

    /// <summary>
    /// 总控保存配置
    /// </summary>
    public async Task SaveSettings(ControlCenter controlCenter)
    {
      var settings = await GetSettings();
      if (settings != null) // 修改
      {
        controlCenter.Id = settings.Id;
        await ControlCenterDb.Update(controlCenter);
      }
      else  // 添加
      {
        await ControlCenterDb.Add(controlCenter);
      }
    }
  }
}
