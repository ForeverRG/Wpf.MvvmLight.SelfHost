using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class ConfigServices : BaseServices<Config>, IConfigServices
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Config>> GetSettings()
    {
      return await ConfigDb.Query();
    }

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<IList<Config>> GetSettings(Expression<Func<Config, bool>> expression)
    {
      return (await ConfigDb.Query(expression)).ToList();
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    public async Task SaveSettings(IList<Config> configs)
    {
      var settings = await GetSettings();
      if (settings.Count == 0)
      {
        await ConfigDb.Add(configs.ToList());
      }
      else
      {
        await ConfigDb.Update(configs);
      }
    }
  }
}
