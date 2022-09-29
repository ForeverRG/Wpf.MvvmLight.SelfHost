using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface ITrayServices : IBaseServices<Tray>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<IList<Tray>> GetSettings();

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<IList<Tray>> GetSettings(Expression<Func<Tray, bool>> expression);

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(IList<Tray> trays);
  }
}
