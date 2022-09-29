using Wpf.MvvmLight.SelfHost.Model;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IVisionServices : IBaseServices<VisionModel>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<VisionModel> GetSettings();

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(VisionModel vision);
  }
}
