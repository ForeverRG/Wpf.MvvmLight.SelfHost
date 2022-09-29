using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class ChannelServices : BaseServices<Channel>, IChannelServices
  {
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Channel>> GetSettings()
    {
      return await ChannelDb.Query();
    }

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<IList<Channel>> GetSettings(Expression<Func<Channel, bool>> expression)
    {
      return (await ChannelDb.Query(expression)).ToList();
    }
  }
}
