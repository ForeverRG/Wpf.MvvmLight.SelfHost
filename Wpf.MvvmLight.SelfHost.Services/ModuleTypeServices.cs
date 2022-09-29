using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class ModuleTypeServices : BaseServices<ModuleType>, IModuleTypeServices
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<IList<ModuleType>> GetSettings()
    {
      return await ModuleTypeDb.Query();
    }

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<IList<ModuleType>> GetSettings(Expression<Func<ModuleType, bool>> expression)
    {
      return (await ModuleTypeDb.Query(expression)).ToList();
    }
  }
}
