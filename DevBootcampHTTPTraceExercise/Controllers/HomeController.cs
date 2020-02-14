using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Threading;

namespace DevBootcampHTTPTraceExercise.Controllers
{
    public class HomeController : Controller
    {

        public async Task<ActionResult> Index()
        {
            string url = "http://gabefakesamesitetest.azurewebsites.net";

            var baseAddress = new Uri(url);

            using (var client = new HttpClient() { BaseAddress = baseAddress })
            {
                var content = new FormUrlEncodedContent(new[]
                {
        new KeyValuePair<string, string>("fakeKey1", "fakeValue1"),
        new KeyValuePair<string, string>("fakeKey2", "fakeValue2"),
    });
                
                var result = await client.PostAsync("/", content);
                result.EnsureSuccessStatusCode();
            }

            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var message = new HttpRequestMessage(HttpMethod.Post, "/Default.aspx");
                message.Headers.Add("Connection", "keep-alive");
                message.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.93 Safari/537.36");
                message.Headers.Add("Accept-Encoding", "gzip, deflate");

                var result = await client.SendAsync(message);
                System.Diagnostics.Trace.WriteLine(result.StatusCode);
                System.Diagnostics.Trace.WriteLine(result.Content);
            }

            return View();
        }
    }
}
 