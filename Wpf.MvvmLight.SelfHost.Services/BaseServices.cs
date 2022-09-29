using Wpf.MvvmLight.SelfHost.Repository.DBSeed;
using Wpf.MvvmLight.SelfHost.IServices;
using System;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class BaseServices<T> : SQLiteDbContext, IBaseServices<T> where T : class, new()
  {
    public void Excute()
    {
      throw new NotImplementedException();
    }

    public void GetCommand()
    {
      throw new NotImplementedException();
    }

    public void SendCommand()
    {
      throw new NotImplementedException();
    }
  }
}
