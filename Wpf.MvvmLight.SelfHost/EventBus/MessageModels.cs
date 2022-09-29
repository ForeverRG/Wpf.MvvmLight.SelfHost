namespace Quectel.ATE.UR.UI.EventBus
{
  public class MessageModels
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
  }
}
