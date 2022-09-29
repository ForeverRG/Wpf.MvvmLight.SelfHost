using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class GripperLoadInfoServices : BaseServices<GripperLoadInfo>, IGripperLoadInfoServices
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<IList<GripperLoadInfo>> GetSettings()
    {
      return await GripperLoadInfoDb.Query();
    }

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<IList<GripperLoadInfo>> GetSettings(Expression<Func<GripperLoadInfo, bool>> expression)
    {
      return (await GripperLoadInfoDb.Query(expression)).ToList();
    }

    /// <summary>
    /// 插入或更新配置
    /// </summary>
    /// <param name="configs"></param>
    /// <returns></returns>
    public async Task SaveSettings(List<GripperLoadInfo> gripperLoadInfos, Expression<Func<GripperLoadInfo, object>> expression)
    {
      await GripperLoadInfoDb.AddOrUpdate(gripperLoadInfos, expression);
    }
  }
}
