using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Wpf.MvvmLight.SelfHost.Api
{
  /// <summary>
  /// HTTP服务
  /// </summary>
  public class HttpService : IDisposable
  {
    /// <summary>
    /// HTTP服务绑定地址
    /// </summary>
    public string BaseAddress { get; private set; } = "http://0.0.0.0";

    /// <summary>
    /// HTTP服务监听端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 直接监听HTTP请求的服务
    /// </summary>
    private readonly HttpSelfHostServer _server;

    public HttpService(int port)
    {
      this.Port = port;

      var config = new HttpSelfHostConfiguration($"{BaseAddress}:{Port}");
      config.MapHttpAttributeRoutes();
      config.Routes.MapHttpRoute("WebApi", "api/{controller}/{action}");

      _server = new HttpSelfHostServer(config);
    }

    /// <summary>
    /// 启动HTTP服务
    /// </summary>
    public Task StartHttpServer()
    {
      return _server.OpenAsync();
    }

    /// <summary>
    /// 关闭HTTP服务
    /// </summary>
    public Task CloseHttpServer()
    {
      return _server.CloseAsync();
    }

    /// <summary>
    /// 执行与释放或重置非托管资源相关联的应用程序定义的任务
    /// </summary>
    public void Dispose()
    {
      _server?.Dispose();
    }
  }
}
