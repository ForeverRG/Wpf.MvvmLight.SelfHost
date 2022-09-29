using Wpf.MvvmLight.SelfHost.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IModuleTypeServices : IBaseServices<ModuleType>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<IList<ModuleType>> GetSettings();

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<IList<ModuleType>> GetSettings(Expression<Func<ModuleType, bool>> expression);
  }
}
