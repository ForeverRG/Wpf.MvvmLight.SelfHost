using Wpf.MvvmLight.SelfHost.IRepository;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Repository.Base;
using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Repository
{
  public class ControlCenterRepository : BaseRepository<ControlCenter>, IControlCenterRepository
  {
    public ControlCenterRepository(ISqlSugarClient context) : base(context)
    {

    }
  }
}
