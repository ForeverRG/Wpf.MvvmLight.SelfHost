using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class VisionServices : BaseServices<VisionModel>, IVisionServices
  {
    /// <summary>
    /// Vision获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<VisionModel> GetSettings()
    {
      return (await VisionDb.Query()).FirstOrDefault();
    }

    /// <summary>
    /// Vison保存配置
    /// </summary>
    public async Task SaveSettings(VisionModel vision)
    {
      var settings = await GetSettings();
      if (settings != null) // 修改
      {
        vision.Id = settings.Id;
        await VisionDb.Update(vision);
      }
      else  // 添加
      {
        await VisionDb.Add(vision);
      }
    }
  }
}
