using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Repository.Base;
using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Repository
{
  public class ChannelRepository : BaseRepository<Channel>, IChannelRepository
  {
    public ChannelRepository(ISqlSugarClient context) : base(context)
    {
    }
  }
}
