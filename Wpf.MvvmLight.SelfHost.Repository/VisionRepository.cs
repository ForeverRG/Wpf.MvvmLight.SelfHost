using Wpf.MvvmLight.SelfHost.IRepository;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.Repository.Base;
using SqlSugar;

namespace Wpf.MvvmLight.SelfHost.Repository
{
  public class VisionRepository : BaseRepository<VisionModel>, IVisionRepository
  {
    public VisionRepository(ISqlSugarClient context) : base(context)
    {

    }
  }
}
