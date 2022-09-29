using Wpf.MvvmLight.SelfHost.Model;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IRobotServices : IBaseServices<Robot>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<Robot> GetSettings();

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(Robot robot);
  }
}
