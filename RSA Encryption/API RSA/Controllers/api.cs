using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSA_Structures;

namespace API_RSA.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class api : ControllerBase
    {
        RSA rsa = new RSA();

        [HttpGet]
        [Route("rsa/keys/{p}/{q}")]
        public async Task<ActionResult> GenerateKeys([FromForm] IFormFile file, int p, int q)
        {            
            string keys = rsa.GetKeys(p,q);
            string[] split = keys.Split("|");
            string publicKey = split[0];
            string privateKey = split[1];
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