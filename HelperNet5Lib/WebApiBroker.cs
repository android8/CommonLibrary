using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace VsscHelper_CoreLibrary
{
  /// <summary>
  /// The service URL supplied by calling application.
  /// HttpClient is used to make the service call.
  /// </summary> 
  public static class WebApiBroker
  {
    /// <summary>
    /// http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
    /// </summary>
    /// <param name="svcLocation"></param>
    /// <returns></returns>
    public static HttpClient GetClient(string svcLocation)
    {
      string myUri = svcLocation;
      CredentialCache cc = new CredentialCache();
      cc.Add(new Uri(myUri), "NTLM", CredentialCache.DefaultNetworkCredentials);

      HttpClientHandler handler = new HttpClientHandler();
      handler.Credentials = cc;
      HttpClient client = new HttpClient(handler);
      client.BaseAddress = new Uri(myUri);
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      return client;
    }

    public static async Task<T> Get<T>(string baseUrl, string urlSegment)
    {
      string content = string.Empty;
      using (HttpClient client = GetClient(baseUrl))
      {
        //reference http://blog.stephencleary.com/2012/07/dont-block-on-async-code.html

        //Continuation of Get resume on the same context which might cause deadlock
        //HttpResponseMessage response = await client.GetAsync(urlSegment.TrimStart('/'));

        //Continuation of Get resume on a thread pool thread
        HttpResponseMessage response = await client.GetAsync(urlSegment.TrimStart('/')).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
          content = await response.Content.ReadAsStringAsync();
          /*
          When ReadAsAsync is called with no parameters, the method uses the default set of media-type formatters to read the response body. 
          The default formatters support JSON, XML, and Form-url-encoded data.

          You can also specify a list of formatters, which is useful if you have a custom media-type formatter:
          var formatters = new List<MediaTypeFormatter>() {
              new MyCustomFormatter(), 
              new JsonMediaTypeFormatter(),
              new XmlMediaTypeFormatter()
          };
          resp.Content.ReadAsAsync<IEnumerable<Product>>(formatters);
          */
        }
        return JsonConvert.DeserializeObject<T>(content);
      }
    }

    public static async Task<T> Post<T>(string baseUrl, string urlSegment, HttpContent postContent)
    {
      //Uri returnUrl = null;
      string content = string.Empty;
      using (HttpClient client = GetClient(baseUrl))
      {
        //HttpResponseMessage response = await client.PostAsync(urlSegment.TrimStart('/'), postContent);
        HttpResponseMessage response = await client.PostAsync(urlSegment.TrimStart('/'), postContent).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
          // returnUrl = response.Headers.Location;
          content = await response.Content.ReadAsStringAsync();
        }
        // return JsonConvert.DeserializeObject<T>(returnUrl.ToString());
        return JsonConvert.DeserializeObject<T>(content);

        /*
         // HTTP POST
          var gizmo = new Product() { Name = "Gizmo", Price = 100, Category = "Widget" };
          response = await client.PostAsJsonAsync("api/products", gizmo);
          if (response.IsSuccessStatusCode)
          {
              // Get the URI of the created resource.
              Uri gizmoUrl = response.Headers.Location;
          }

          The PostAsJsonAsync method serializes an object to JSON and then sends the JSON payload in a POST request. To send XML, use the PostAsXmlAsync method. To use another formatter, call PostAsync:
          MediaTypeFormatter formatter = new MyCustomFormatter();
          response = await client.PostAsync("api/products", gizmo, formatter);

         */
      }
    }

    public static async Task<T> Delete<T>(string baseUrl, string urlSegment)
    {
      string responseContent = string.Empty;
      HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

      using (HttpClient client = GetClient(baseUrl))
      {
        HttpResponseMessage response = await client.DeleteAsync(urlSegment.TrimStart('/')).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
          statusCode = response.StatusCode;
        }
        return JsonConvert.DeserializeObject<T>(statusCode.ToString());
      }
    }

    /// <summary> 
    /// Start to Call WCF service 
    /// </summary> 
    /// <param name="sender"></param> 
    /// <param name="e"></param> 
    //private async void Start_Click(object sender, RoutedEventArgs e) 
    //{ 
    //    // Clear text of Output textbox  
    //    this.OutputField.Text = string.Empty; 
    //    this.StatusBlock.Text = string.Empty; 


    //    this.StartButton.IsEnabled = false; 
    //    httpClient = new HttpClient(); 
    //    try 
    //    { 
    //        string resourceAddress = "http://localhost:44516/WCFService.svc/GetData"; 
    //        int age = Convert.ToInt32(this.Agetxt.Text); 
    //        if (age > 120 || age < 0) 
    //        { 
    //            throw new Exception("Age must be between 0 and 120"); 
    //        } 
    //        Person p = new Person { Name = this.Nametxt.Text, Age = age }; 
    //        string postBody = JsonSerializer(p); 
    //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
    //        HttpResponseMessage wcfResponse = await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json")); 
    //        await DisplayTextResult(wcfResponse, OutputField); 
    //    } 
    //    catch (HttpRequestException hre) 
    //    { 
    //        NotifyUser("Error:" + hre.Message); 
    //    } 
    //    catch (TaskCanceledException) 
    //    { 
    //        NotifyUser("Request canceled."); 
    //    } 
    //    catch (Exception ex) 
    //    { 
    //        NotifyUser(ex.Message); 
    //    } 
    //    finally 
    //    { 
    //        this.StartButton.IsEnabled = true; 
    //        if (httpClient != null) 
    //        { 
    //            httpClient.Dispose(); 
    //            httpClient = null; 
    //        } 
    //    } 
    //} 

  }

  /// <summary>
  /// abstract version of the wrapper cannot be initiated but must be inherited
  /// </summary>
  public abstract class WebClientWrapperBase : IDisposable
  {
    private readonly string _baseUrl;
    private Lazy<WebClient> _lazyClient;

    protected WebClientWrapperBase(string baseUrl)
    {
      _baseUrl = baseUrl.Trim('/');
      _lazyClient = new Lazy<WebClient>(() => new WebClient());
    }

    protected WebClient Client()
    {
      if (_lazyClient == null)
      {
        throw new ObjectDisposedException("WebClient has been disposed");
      }

      return _lazyClient.Value;
    }

    protected T Execute<T>(string urlSegment)
    {
      return JsonConvert.DeserializeObject<T>(Client().DownloadString(_baseUrl + '/' + urlSegment.TrimStart('/')));
    }

    ~WebClientWrapperBase()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(false);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (_lazyClient != null)
      {
        if (disposing)
        {
          if (_lazyClient.IsValueCreated)
          {
            _lazyClient.Value.Dispose();
            _lazyClient = null;
          }
        }

        // There are no unmanaged resources to release, but
        // if we add them, they need to be released here.
      }
    }
  }

  /// <summary>
  /// static version of the wrapper.  Only one instance is initiated by calling program
  /// </summary>
  public static class WebClientWrapper
  {
    static private string _baseUrl;
    static private Lazy<WebClient> _lazyClient;

    static private WebClient Client()
    {
      if (_lazyClient == null)
      {
        throw new ObjectDisposedException("WebClient has been disposed");
      }

      //string myUri = _baseUrl;
      //CredentialCache cc = new CredentialCache();
      //cc.Add(new Uri(myUri), "NTLM", CredentialCache.DefaultNetworkCredentials);
      //_lazyClient.Value.Credentials = cc;
      return _lazyClient.Value;
    }

    static public T Execute<T>(string baseUrl, string urlSegment)
    {
      _baseUrl = baseUrl.Trim('/');
      _lazyClient = new Lazy<WebClient>(() => new WebClient { UseDefaultCredentials = true });
      return JsonConvert.DeserializeObject<T>(Client().DownloadString(_baseUrl + '/' + urlSegment.TrimStart('/')));
    }
  }
}
