namespace Wpf.MvvmLight.SelfHost.Api.Model
{
  /// <summary>
  /// 通用返回信息类
  /// </summary>
  public class HttpMessage<T>
  {
    /// <summary>
    /// 状态码
    /// </summary>
    public int Status { get; set; } = 200;
    /// <summary>
    /// 操作是否成功
    /// </summary>
    public bool Success { get; set; } = false;
    /// <summary>
    /// 返回信息
    /// </summary>
    public string Msg { get; set; } = "";
    /// <summary>
    /// 开发者信息
    /// </summary>
    public string MsgDev { get; set; }
    /// <summary>
    /// 返回数据集合
    /// </summary>
    public T Response { get; set; }

    /// <summary>
    /// 返回成功
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public static HttpMessage<T> SuccessMsg(string msg)
    {
      return Message(true, msg, default);
    }
    /// <summary>
    /// 返回成功
    /// </summary>
    /// <param name="msg">消息</param>
    /// <param name="response">数据</param>
    /// <returns></returns>
    public static HttpMessage<T> SuccessMsg(string msg, T response)
    {
      return Message(true, msg, response);
    }
    /// <summary>
    /// 返回失败
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public static HttpMessage<T> FailMsg(string msg)
    {
      return Message(false, msg, default);
    }
    /// <summary>
    /// 返回失败
    /// </summary>
    /// <param name="msg">消息</param>
    /// <param name="response">数据</param>
    /// <returns></returns>
    public static HttpMessage<T> FailMsg(string msg, T response)
    {
      return Message(false, msg, response);
    }

    /// <summary>
    /// 返回消息
    /// </summary>
    /// <param name="success">失败/成功</param>
    /// <param name="msg">消息</param>
    /// <param name="response">数据</param>
    /// <returns></returns>
    public static HttpMessage<T> Message(bool success, string msg, T response)
    {
      return new HttpMessage<T>() { Msg = msg, Response = response, Success = success };
    }
  }

  public class MessageModel
  {
    /// <summary>
    /// 状态码
    /// </summary>
    public int Status { get; set; } = 200;
    /// <summary>
    /// 操作是否成功
    /// </summary>
    public bool Success { get; set; } = false;
    /// <summary>
    /// 返回信息
    /// </summary>
    public string Msg { get; set; } = "";
    /// <summary>
    /// 返回数据集合
    /// </summary>
    public object Response { get; set; }
  }
}
