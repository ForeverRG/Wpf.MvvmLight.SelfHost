namespace Wpf.MvvmLight.SelfHost.IServices
{
  public interface IBaseServices<T> where T : class, new()
  {
    /// <summary>
    /// 获取命令
    /// </summary>
    void GetCommand();
    /// <summary>
    /// 发送命令
    /// </summary>
    void SendCommand();
    /// <summary>
    /// 执行任务
    /// </summary>
    void Excute();
  }
}
