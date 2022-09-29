using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class RobotServices : BaseServices<Robot>, IRobotServices
  {
    /// <summary>
    /// robot获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<Robot> GetSettings()
    {
      return (await RobotDb.Query()).FirstOrDefault();
    }

    /// <summary>
    /// Robot保存配置
    /// </summary>
    public async Task SaveSettings(Robot robot)
    {
      var settings = await GetSettings();
      if (settings != null) // 修改
      {
        robot.Id = settings.Id;
        await RobotDb.Update(robot);
      }
      else  // 添加
      {
        await RobotDb.Add(robot);
      }
    }
  }
}
