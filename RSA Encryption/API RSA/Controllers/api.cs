using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSA_Structures;
using System.IO.Compression;

namespace API_RSA.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class api : ControllerBase
    {
        private IWebHostEnvironment _env;
        public api(IWebHostEnvironment env)
        {
            _env = env;
        }

        RSA rsa = new RSA();

        [HttpGet]
        [Route("rsa/keys/{p}/{q}")]
        public ActionResult GenerateKeys(int p, int q)
        {
            try
            {
                string keys = rsa.GetKeys(p, q);
                string[] split = keys.Split("|");
                string publicKey = split[0];
                string privateKey = split[1];

                string pathPublicKey = _env.ContentRootPath + "/Keys/public.key";
                string pathPrivateKey = _env.ContentRootPath + "/Keys/private.key";

                if (System.IO.File.Exists(pathPublicKey))
                {
                    System.IO.File.Delete(pathPublicKey);
                }
                if (System.IO.File.Exists(pathPrivateKey))
                {
                    System.IO.File.Delete(pathPrivateKey);
                }
                
                using (FileStream fs = System.IO.File.Create(pathPublicKey))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(publicKey);
                    fs.Write(info, 0, info.Length);
                }
                using (FileStream fs = System.IO.File.Create(pathPrivateKey))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(privateKey);
                    fs.Write(info, 0, info.Length);
                }

                string pathZip = _env.ContentRootPath + "/Keys/Keys.zip";

                using (ZipArchive archive = ZipFile.Open(pathZip, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(pathPublicKey, "public.key");
                    archive.CreateEntryFromFile(pathPrivateKey, "private.key");
                }

                byte[] readBytes = System.IO.File.ReadAllBytes(pathZip);
                if (System.IO.File.Exists(pathZip)) System.IO.File.Delete(pathZip);
                
                return File(readBytes, "application/zip", "Keys.zip");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }                            
        }

        [HttpPost]
        [Route("rsa/{nombre}")]
        public async Task<ActionResult> RSA([FromForm] IFormFile file, [FromForm] IFormFile keys, string nombre)
        {
            try
            {
                string extension = file.FileName.Substring(file.FileName.Length -4, 4);  

                byte[] originalBytes = null;
                using (var memory = new MemoryStream())
                {
                    await file.CopyToAsync(memory);
                    originalBytes = memory.ToArray();
                }

                byte[] keyBytes = null;
                using (var memory = new MemoryStream())
                {
                    await keys.CopyToAsync(memory);
                    keyBytes = memory.ToArray();
                }
                string auxKeys = Encoding.Default.GetString(keyBytes);
                string[] splits = auxKeys.Split(",");

                int isEncrypted = -1;
                byte[] result = rsa.RSA_(originalBytes, Convert.ToInt32(splits[0]), Convert.ToInt32(splits[1]), ref isEncrypted, extension);

                if (isEncrypted == 1)
                {
                    return File(result, "compressedFile / rsa", nombre + ".rsa");
                }
                else
                {
                    return File(result, "compressedFile / txt", nombre + ".txt");
                }             
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}