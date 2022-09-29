namespace Wpf.MvvmLight.SelfHost.EventBus
{
  public class MessageModel
  {
    /// <summary>
    /// 定位夹具消息
    /// </summary>
    public class PositioningSocket
    {
      public string SocketType { get; set; }
      public int JigNumber { get; set; }
    }

    /// <summary>
    /// 更新夹具Socket左边偏移值消息
    /// </summary>
    public class JigSocketOffset
    {
      public PositioningSocket PositioningSocket { get; set; }
      public string[] OffsetArr { get; set; }
    }

    /// <summary>
    /// 更新夹具二维码消息
    /// </summary>
    public class JigSocketQR
    {
      public PositioningSocket PositioningSocket { get; set; }
      public string QR { get; set; }
    }

    /// <summary>
    /// 弹框消息
    /// </summary>
    public class MessageBoxTunnel
    {
      /// <summary>
      /// 弹框显示内容
      /// </summary>
      public string Message { get; set; }
      /// <summary>
      /// 结果回传标志
      /// </summary>
      public string EchoToken { get; set; }
    }
  }
}
