using Wpf.MvvmLight.SelfHost.IRepository;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Repository.Base;
using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Repository
{
  public class JigRepository : BaseRepository<Jig>, IJigRepository
  {
    public JigRepository(ISqlSugarClient context) : base(context)
    {
    }
  }
}
