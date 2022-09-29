using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class PLCServices : BaseServices<PLC>, IPLCServices
  {
    /// <summary>
    /// PLC获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<PLC> GetSettings()
    {
      return (await PLCDb.Query()).FirstOrDefault();
    }

    /// <summary>
    /// PLC保存配置
    /// </summary>
    public async Task SaveSettings(PLC plc)
    {
      var settings = await GetSettings();
      if (settings != null) // 修改
      {
        plc.Id = settings.Id;
        await PLCDb.Update(plc);
      }
      else  // 添加
      {
        await PLCDb.Add(plc);
      }
    }
  }
}
