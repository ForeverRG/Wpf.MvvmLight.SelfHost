using GalaSoft.MvvmLight;
using SuperSocket.SocketBase;

namespace Wpf.MvvmLight.SelfHost.Common
{
  public class HomeModel : ObservableObject
  {
    /// <summary>
    /// 总控服务
    /// </summary>
    public AppServer ControlCenterServer { get; set; }
    /// <summary>
    /// 视觉客户端
    /// </summary>
    public AppSession VisionClient { get; set; }
    /// <summary>
    /// UR客户端
    /// </summary>
    public AppSession RobotClient { get; set; }
    /// <summary>
    /// PLC客户端
    /// </summary>
    public AppSession PLCClient { get; set; }

    private bool isEnableStartBtn;
    private bool isEnableSuspendOrContinueBtn;
    private bool isEnableStopBtn;
    private string scBtnContent;

    /// <summary>
    /// 启动总控按钮
    /// </summary>
    public bool IsEnableStartBtn
    {
      get { return isEnableStartBtn; }
      set { isEnableStartBtn = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 暂停/继续总控按钮
    /// </summary>
    public bool IsEnableSuspendOrContinueBtn
    {
      get { return isEnableSuspendOrContinueBtn; }
      set { isEnableSuspendOrContinueBtn = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 停止总控按钮
    /// </summary>
    public bool IsEnableStopBtn
    {
      get { return isEnableStopBtn; }
      set { isEnableStopBtn = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 暂停/继续按钮文字
    /// </summary>
    public string SCBtnContent
    {
      get { return scBtnContent; }
      set { scBtnContent = value; RaisePropertyChanged(); }
    }
  }
}
