﻿using DotNetCore.Interface;
using Microsoft.Extensions.Configuration;
using Nest;
using System;

namespace DotNetCore.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IConfiguration _configuration;
        private ElasticClient _client;

        public ElasticsearchService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ElasticClient GetClient()
        {
            if (_client != null)
            {
                return _client;
            }

            InitClient();
            return _client;
        }

        private void InitClient()
        {
            var node = new Uri(_configuration["EsUrl"]);
            _client = new ElasticClient(new ConnectionSettings(node).DefaultIndex("demo"));
        }
    }
}

