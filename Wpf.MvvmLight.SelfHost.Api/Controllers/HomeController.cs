using System.Web.Http;
using Wpf.MvvmLight.SelfHost.Api.Model;

namespace Wpf.MvvmLight.SelfHost.Api.Controllers
{
  /// <summary>
  /// Home controller.
  /// </summary>
  [RoutePrefix("api/v1/home")]
  public class HomeController : ApiController
  {
    [Route("echo")]
    [HttpGet]
    public IHttpActionResult EchoGet(string name)
    {
      return Json(new { Name = name, Message = $"Hello {name}" });
    }

    [Route("echo")]
    [HttpPost]
    public IHttpActionResult EchoPost([FromBody] PostEcho echo)
    {
      return Json(new { Name = echo.Name, Message = $"Hello {echo.Name}" });
    }
  }
}
