using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.ObjectModel;

namespace Wpf.MvvmLight.SelfHost.Common.ASChildModel
{
  public class JigModel : ObservableObject
  {
    private ObservableCollection<Jig> jigsQR;
    private ObservableCollection<Jig> jigsSocket1;
    private ObservableCollection<Jig> jigsSocket2;

    public ObservableCollection<Jig> JigsQR
    {
      get { return jigsQR; }
      set { jigsQR = value; RaisePropertyChanged(); }
    }
    public ObservableCollection<Jig> JigsSocket1
    {
      get { return jigsSocket1; }
      set { jigsSocket1 = value; RaisePropertyChanged(); }
    }
    public ObservableCollection<Jig> JigsSocket2
    {
      get { return jigsSocket2; }
      set { jigsSocket2 = value; RaisePropertyChanged(); }
    }
  }
}
