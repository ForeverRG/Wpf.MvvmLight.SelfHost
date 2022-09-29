using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class ModuleSettingsServices : BaseServices<ModuleSettings>, IModuleSettingsServices
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<IList<ModuleSettings>> GetSettings()
    {
      return await ModuleSettingsDb.Query();
    }

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<IList<ModuleSettings>> GetSettings(Expression<Func<ModuleSettings, bool>> expression)
    {
      return (await ModuleSettingsDb.Query(expression)).ToList();
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    public async Task SaveSettings(IList<ModuleSettings> moduleSettings)
    {
      var settings = await GetSettings();
      if (settings.Count == 0)
      {
        await ModuleSettingsDb.Add(moduleSettings.ToList());
      }
      else
      {
        await ModuleSettingsDb.Update(moduleSettings);
      }
    }
  }
}
