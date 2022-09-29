using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IConfigServices : IBaseServices<Config>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<IList<Config>> GetSettings();

    Task<IList<Config>> GetSettings(Expression<Func<Config, bool>> expression);

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(IList<Config> configs);
  }
}
