using RestSharp;
using System;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.Common.HttpRequestHelper
{
  public class RangeRestApi<T> where T : class, new()
  {
    #region 同步
    public T Get(string url, object pars = null)
    {
      var type = Method.Get;
      RestResponse<T> reval = GetApiInfo(url, pars, type);
      return reval.Data;
    }
    public T Post(string url, object pars = null)
    {
      var type = Method.Post;
      RestResponse<T> reval = GetApiInfo(url, pars, type);
      return reval.Data;
    }
    public T Delete(string url, object pars = null)
    {
      var type = Method.Delete;
      RestResponse<T> reval = GetApiInfo(url, pars, type);
      return reval.Data;
    }
    public T Put(string url, object pars = null)
    {
      var type = Method.Put;
      RestResponse<T> reval = GetApiInfo(url, pars, type);
      return reval.Data;
    }

    private static RestResponse<T> GetApiInfo(string url, object pars, Method type)
    {
      var request = new RestRequest();
      request.Method = type;
      if (pars != null)
      {
        request.AddObject(pars);
      }
      var client = new RestClient(url);
      RestResponse<T> reval = client.Execute<T>(request);
      if (reval.ErrorException != null)
      {
        throw new Exception($"{type}请求出错:{reval.ErrorException.Message}");
      }
      return reval;
    }

    #endregion

    #region 异步

    public async Task<T> GetAsync(string url, object pars = null)
    {
      var type = Method.Get;
      RestResponse<T> reval = await GetApiInfoAsync(url, pars, type);
      return reval.Data;
    }
    public async Task<T> PostAsync(string url, object pars = null)
    {
      var type = Method.Post;
      RestResponse<T> reval = await GetApiInfoAsync(url, pars, type);
      return reval.Data;
    }
    public async Task<T> DeleteAsync(string url, object pars = null)
    {
      var type = Method.Delete;
      RestResponse<T> reval = await GetApiInfoAsync(url, pars, type);
      return reval.Data;
    }
    public async Task<T> PutAsync(string url, object pars = null)
    {
      var type = Method.Put;
      RestResponse<T> reval = await GetApiInfoAsync(url, pars, type);
      return reval.Data;
    }

    private static async Task<RestResponse<T>> GetApiInfoAsync(string url, object pars, Method type)
    {
      var request = new RestRequest();
      request.Method = type;
      if (pars != null)
      {
        request.AddObject(pars);
      }
      var client = new RestClient(url);
      RestResponse<T> reval = await client.ExecuteAsync<T>(request);
      if (reval.ErrorException != null)
      {
        throw new Exception($"{type}请求出错:{reval.ErrorException.Message}");
      }
      return reval;
    }

    #endregion
  }

  public class RangeRestApi
  {
    #region 同步

    public string Get(string url, object pars = null)
    {
      var type = Method.Get;
      RestResponse reval = GetApiInfo(url, pars, type);
      return reval.Content;
    }
    public string Post(string url, object pars = null)
    {
      var type = Method.Post;
      RestResponse reval = GetApiInfo(url, pars, type);
      return reval.Content;
    }
    public string Delete(string url, object pars = null)
    {
      var type = Method.Delete;
      RestResponse reval = GetApiInfo(url, pars, type);
      return reval.Content;
    }
    public string Put(string url, object pars = null)
    {
      var type = Method.Put;
      RestResponse reval = GetApiInfo(url, pars, type);
      return reval.Content;
    }

    private static RestResponse GetApiInfo(string url, object pars, Method type)
    {
      var request = new RestRequest();
      request.Method = type;
      if (pars != null)
      {
        request.AddObject(pars);
      }
      var client = new RestClient(url);
      RestResponse reval = client.Execute(request);
      if (reval.ErrorException != null)
      {
        throw new Exception($"{type}请求出错:{reval.ErrorException.Message}");
      }
      return reval;
    }

    #endregion

    #region 异步

    public async Task<string> GetAsync(string url, object pars = null)
    {
      var type = Method.Get;
      RestResponse reval = await GetApiInfoAsync(url, pars, type);
      return reval.Content;
    }
    public async Task<string> PostAsync(string url, object pars = null)
    {
      var type = Method.Post;
      RestResponse reval = await GetApiInfoAsync(url, pars, type);
      return reval.Content;
    }
    public async Task<string> DeleteAsync(string url, object pars = null)
    {
      var type = Method.Delete;
      RestResponse reval = await GetApiInfoAsync(url, pars, type);
      return reval.Content;
    }
    public async Task<string> PutAsync(string url, object pars = null)
    {
      var type = Method.Put;
      RestResponse reval = await GetApiInfoAsync(url, pars, type);
      return reval.Content;
    }

    private static async Task<RestResponse> GetApiInfoAsync(string url, object pars, Method type)
    {
      var request = new RestRequest();
      request.Method = type;
      if (pars != null)
      {
        request.AddObject(pars);
      }
      var client = new RestClient(url);
      RestResponse reval = await client.ExecuteAsync(request);
      if (reval.ErrorException != null)
      {
        throw new Exception($"{type}请求出错:{reval.ErrorException.Message}");
      }
      return reval;
    }

    #endregion
  }
}
