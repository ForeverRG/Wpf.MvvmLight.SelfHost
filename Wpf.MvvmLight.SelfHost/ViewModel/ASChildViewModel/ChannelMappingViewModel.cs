using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.Common.ASChildModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Wpf.MvvmLight.SelfHost.IServices;

namespace Wpf.MvvmLight.SelfHost.ViewModel.ASChildViewModel
{
  public class ChannelMappingViewModel : ViewModelBase, IEventBus
  {
    private IChannelServices _channelServices;
    private ChannelMappingModel _channelMappingModel;

    public ChannelMappingModel ChannelMappingModel { get => _channelMappingModel; set { _channelMappingModel = value; RaisePropertyChanged(); } }

    public ChannelMappingViewModel(IChannelServices channelServices)
    {
      _channelServices = channelServices;
      ChannelMappingModel = new ChannelMappingModel();

      RegisterMessageSignal();

      EventSignal.SendShowProgressViewSignal(true);
      Task.Run(async () =>
      {
        await InitChannelMappingModel();
        EventSignal.SendShowProgressViewSignal(false);
      });
    }

    /// <summary>
    /// 初始化机型设置模型
    /// </summary>
    private async Task InitChannelMappingModel()
    {
      ChannelMappingModel.Sides = new List<string> { "Socket1", "Socket2" };

      var channel = new ObservableCollection<Channel>(await _channelServices.GetSettings());
      var defaultChannel = new ObservableCollection<Channel>(Enumerable.Range(1, 16).Select(
            i => new Channel
            {
              Id = i,
              Number = i,
              Socket = "Socket1",
              TestBenchNumber = i,
            }));
      ChannelMappingModel.Channels = channel.Count > 0 ? channel : defaultChannel;
    }

    public void RegisterMessageSignal()
    {
      //MessengerInstance.Register<string>(this, "SaveAdvancedSettingsSignal", SaveAdvancedSettingsSlot);
    }

    //private void SaveAdvancedSettingsSlot(string obj)
    //{
    //  // TODO:保存通道映射配置
    //}

    public void UnregisterMessageSignal()
    {
      throw new System.NotImplementedException();
    }
  }
}
