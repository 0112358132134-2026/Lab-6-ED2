using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_RSA.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class api : ControllerBase
    {
        [HttpGet]
        [Route("rsa/keys/{p}/{q}")]
        public async Task<ActionResult> GenerateKeys([FromForm] IFormFile file, int p, int q)
        {
            return Ok();        
        }

        [HttpPost]
        [Route("rsa/{nombre}")]
        public async Task<ActionResult> RSA([FromForm] IFormFile file, string nombre)
        {
            return Ok();
        }
    }
}