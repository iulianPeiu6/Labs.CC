using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UScheduler.WebApi.Storage.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UScheduler.WebApi.Storage.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly StorageClient storage;
        private readonly CloudStorageOptions config;

        public StorageController(IOptions<CloudStorageOptions> config)
        {
            this.config = config.Value;
            var googleCredentials = GoogleCredential.FromFile("uscheduler-345313-91a35f638c58.json");
            storage = StorageClient.Create(googleCredentials);
        }

        [HttpPost]
        public async Task<Object> Add([FromBody] BucketObject bucketObject)
        {
            var response = await storage.UploadObjectAsync(
                config.BucketName,
                bucketObject.ObjectName, 
                "text/plain", 
                new MemoryStream(Encoding.UTF8.GetBytes(bucketObject.Content)));

            return response;
        }
    }
}
