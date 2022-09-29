using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using Wpf.MvvmLight.SelfHost.Common.Model;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class TrayServices : BaseServices<Tray>, ITrayServices
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Tray>> GetSettings()
    {
      return await TrayDb.Query();
    }

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<IList<Tray>> GetSettings(Expression<Func<Tray, bool>> expression)
    {
      return (await TrayDb.Query(expression)).ToList();
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    public async Task SaveSettings(IList<Tray> trays)
    {
      var settings = await GetSettings();
      if (settings.Count == 0)
      {
        await TrayDb.Add(trays.ToList());
      }
      else
      {
        await TrayDb.Update(trays);
      }
    }
  }
}

