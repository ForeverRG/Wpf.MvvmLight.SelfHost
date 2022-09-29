namespace Wpf.MvvmLight.SelfHost.EventBus
{
  /// <summary>
  /// 发布/订阅消息机制
  /// </summary>
  public interface IEventBus
  {
    /// <summary>
    /// 注册当前(this)对象注册的所有MVVMLight消息信号
    /// </summary>
    void RegisterMessageSignal();

    /// <summary>
    /// 注销当前(this)对象注册的所有MVVMLight消息信号
    /// </summary>
    void UnregisterMessageSignal();
  }
}
