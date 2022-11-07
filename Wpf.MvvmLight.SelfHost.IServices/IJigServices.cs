using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IJigServices : IBaseServices<Jig>
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    Task<IList<Jig>> GetSettings();

    Task<IList<Jig>> GetSettings(Expression<Func<Jig, bool>> expression);

    /// <summary>
    /// 保存配置
    /// </summary>
    Task SaveSettings(IList<Jig> jigs);

    Task SaveSettings(Jig jig);

    Task SaveSettings(List<Jig> jigs, Expression<Func<Jig, object>> expression);

    bool UpdateJigField(Expression<Func<Jig, Jig>> columsExpression, Expression<Func<Jig, bool>> whereExpression);
  }
}
