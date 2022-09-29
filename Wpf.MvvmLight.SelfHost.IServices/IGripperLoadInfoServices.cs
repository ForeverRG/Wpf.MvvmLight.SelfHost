using Wpf.MvvmLight.SelfHost.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IGripperLoadInfoServices : IBaseServices<GripperLoadInfo>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<IList<GripperLoadInfo>> GetSettings();

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<IList<GripperLoadInfo>> GetSettings(Expression<Func<GripperLoadInfo, bool>> expression);
  }
}
