using DotNetCore.Interface;
using DotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Collections.Generic;

namespace DotNetCore.Mvc.Controllers
{
    public class ElasticsearchController : ControllerBase
    {
        private readonly ElasticClient _client;

        public ElasticsearchController(IElasticsearchService clientProvider)
        {
            _client = clientProvider.GetClient();
        }

        //[HttpPost]
        //[Route("value/index")]
        //public IIndexResponse Index(EsRequest post)
        //{
        //    return _client.IndexDocument(post);
        //}

        [HttpGet]
        public IReadOnlyCollection<EsRequest> Search(string type)
        {
            return _client.Search<EsRequest>(s => s
                .From(0)
                .Size(10)
                .Query(q => q.Match(m => m.Field(f => f.Type).Query(type)))).Documents;
        }
    }
}