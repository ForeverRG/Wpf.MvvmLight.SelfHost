namespace Wpf.MvvmLight.SelfHost.EventBus.EventMessageModel
{
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
