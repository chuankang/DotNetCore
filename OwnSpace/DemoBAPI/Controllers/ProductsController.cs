﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // GET: api/Products
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "products1", "products2" };
        }

        // GET: api/Products/5
        [HttpGet("{GetId}", Name = "Get")]
        public string GetId(int id)
        {
            return "value";
        }

        // POST: api/Products
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}