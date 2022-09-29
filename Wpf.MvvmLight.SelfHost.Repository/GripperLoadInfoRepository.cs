using Wpf.MvvmLight.SelfHost.IRepository;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Repository.Base;
using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Repository
{
  public class GripperLoadInfoRepository : BaseRepository<GripperLoadInfo>, IGripperLoadInfoRepository
  {
    public GripperLoadInfoRepository(ISqlSugarClient context) : base(context)
    {
    }
  }
}
