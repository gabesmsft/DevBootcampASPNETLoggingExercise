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
            //This controller will send an outbound request to another service if either of the following conditions are met:
            //1) You send a FakeHeader request header to this controller
            //2) You don't send a X-ARR-SSL request header to this controller
            bool validInboundRequest = (Request.Headers["FakeHeader"] != null || Request.Headers["X-ARR-SSL"] == null);

            if (!validInboundRequest)
            {
                ViewBag.InboundRequestCustomResponse = @"Please do one or more of the following: 1) Send a request with a FakeHeader header, or 2) Ensure that you aren't sending a X-ARR-SSL request header.";
                ViewBag.OutboundRequestCustomResponse = "Not sent because of above error message";
            }

            else
            {
                ViewBag.InboundRequestCustomResponse = "Inbound request is valid";

                try
                {
                    string url = "https://devbootcampfakeexternalservice.azurewebsites.net";

                    var baseAddress = new Uri(url);

                    using (var client = new HttpClient() { BaseAddress = baseAddress })
                    {
                        var message = new HttpRequestMessage(HttpMethod.Get, "/api/fake");
                        message.Headers.Add("FakeHeader2", Request.Url.ToString());
                        var result = await client.SendAsync(message);
                        
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            ViewBag.OutboundRequestCustomResponse = "Outbound request was successfully sent";
                        }
                        else
                        {
                            ViewBag.OutboundRequestCustomResponse = "An issue happened when sending the outbound request";
                        }

                        System.Diagnostics.Trace.WriteLine("Status code response of outbound request: " + result.StatusCode);
                        string content = await result.Content.ReadAsStringAsync();
                        System.Diagnostics.Trace.WriteLine("Response content of outbound request: " + content);

                        //var result = await client.GetAsync("/api/fake");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.OutboundRequestCustomResponse = "An exception happened when trying to send the outbound request";
                }
            }
            return View();
        }
    }
}
 