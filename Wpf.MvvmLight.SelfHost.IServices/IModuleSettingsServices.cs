using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IModuleSettingsServices : IBaseServices<ModuleSettings>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<IList<ModuleSettings>> GetSettings();

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(IList<ModuleSettings> moduleSettings);

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<IList<ModuleSettings>> GetSettings(Expression<Func<ModuleSettings, bool>> expression);

  }
}
