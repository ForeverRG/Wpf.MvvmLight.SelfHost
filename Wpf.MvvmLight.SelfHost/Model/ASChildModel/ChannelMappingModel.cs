using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Wpf.MvvmLight.SelfHost.Common.ASChildModel
{
  public class ChannelMappingModel : ObservableObject
  {
    private ObservableCollection<Channel> channels;
    private IList<string> sides;

    public ObservableCollection<Channel> Channels
    {
      get { return channels; }
      set { channels = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 夹具端口
    /// </summary>
    public IList<string> Sides { get => sides; set => sides = value; }
  }
}
