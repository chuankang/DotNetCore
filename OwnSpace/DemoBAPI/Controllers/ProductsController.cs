using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace DemoBAPI.Controllers
{
    /// <summary>
    /// 产品API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly RetryPolicy<HttpResponseMessage> _httpRequestPolicy;


        public ProductsController()
        {
            //拿到响应中状态码，若为500则重试三次。
            _httpRequestPolicy = Policy.HandleResult<HttpResponseMessage>(
                    r => r.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromSeconds(retryAttempt));
        }



        // GET: api/Products
        /// <summary>
        /// 无参Get
        /// </summary>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "product1", "product2" };
        }

        /// <summary>
        /// 产品获得ID
        /// </summary>
        [HttpGet("{GetId}")]
        public string GetId(int id)
        {
            return "张三的Id";
        }

        [HttpGet("{GetResult}")]
        public async Task<string> GetResult()
        {
            var httpClient = new HttpClient();
            const string requestEndpoint = "http://localhost:5002";

            HttpResponseMessage httpResponse = await 
                _httpRequestPolicy.ExecuteAsync(() => httpClient.GetAsync(requestEndpoint));

            IEnumerable<string> numbers = await httpResponse.Content.ReadAsAsync<IEnumerable<string>>();
            
            return "123";
        }
    }
}
