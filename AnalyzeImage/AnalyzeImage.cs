using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMVisionApiClient.VisionAPI;
using BMVisionApiClient.VisionAPI.Constants;
using BMVisionApiClient.VisionAPI.Enum;
using BMVisionApiClient.VisionAPI.Exceptions;
using BMVisionApiClient.VisionAPI.Models;
using BMVisionApiClient.VisionAPI.Utils;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace AnalyzeImage
{
    public static class AnalyzeImage
    {
        private static string _freeSubscriptionKey;
        private static string _analyzeContainerName;

        [FunctionName("AnalyzeImage")]
        public static void Run(
            [BlobTrigger("fileuploads/{name}", Connection = "Source")]Stream myBlob,
            string name, 
            TraceWriter log)
        {
            if (!name.Contains("_thumb"))
            {
                log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
                log.Info($"No Thumbnail -> Skipping.");
                return;
            }

            _freeSubscriptionKey = System.Configuration.ConfigurationManager.AppSettings["SubscriptionKey"];
            _analyzeContainerName = System.Configuration.ConfigurationManager.AppSettings["Output-Blob-Folder"]; 

            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            InitVisionApiClient();

            Analyze(myBlob, name, log);
            OCR(myBlob, name, log);
        }

        private static void Analyze(Stream myBlob, string name, TraceWriter log)
        {
            myBlob.Seek(0, SeekOrigin.Begin);
            try
            {
                var analyzeTask = Task.Run(async () => await VisionApiClient.AnalyzeImageStreamAsync(myBlob));
                analyzeTask.Wait();
                if (analyzeTask.Status == TaskStatus.RanToCompletion)
                {
                    log.Info($"Analyze Successfull");
                    var result = analyzeTask.Result;
                    log.Info(result.RequestId);
                    var json = JsonConvert.SerializeObject(result);
                    var storeBlobTask = Task.Run(async () => await CreateBlob(_analyzeContainerName, $"{name}.analyze.json", json));
                    storeBlobTask.Wait();
                    log.Info($"Json Stored Successfull");
                }
                else
                {
                    log.Error($"Analyzing Image did fail");
                }

            }
            catch (VisionApiClientException ex)
            {
                log.Error($"Vision API Client Exception", ex);
            }
        }

        private static void OCR(Stream myBlob, string name, TraceWriter log)
        {
            myBlob.Seek(0, SeekOrigin.Begin);
            try
            {
                var analyzeTask = Task.Run(async () => await VisionApiClient.OCRImageStreamAsync(myBlob));
                analyzeTask.Wait();
                if (analyzeTask.Status == TaskStatus.RanToCompletion)
                {
                    log.Info($"OCR Successfull");
                    var result = analyzeTask.Result;
                    var json = JsonConvert.SerializeObject(result);
                    var storeBlobTask = Task.Run(async () => await CreateBlob(_analyzeContainerName, $"{name}.ocr.json", json));
                    storeBlobTask.Wait();
                    log.Info($"Json Stored Successfull");
                }
                else
                {
                    log.Error($"OCR did fail");
                }
            }
            catch (VisionApiClientException ex)
            {
                log.Error($"Vision API Client Exception", ex);
            }
        }

        private static void InitVisionApiClient()
        {
            VisionApiClient.SetSubscriptionKey(_freeSubscriptionKey);
            VisionApiClient.SetRegion(AzureRegion.WestEurope);
            VisionApiClient.SetDefaultQueryParameters(new QueryParameters
            {
                VisualFeatures = EnumUtil.GetValues<VisualFeature>().ToList(),
                Details = EnumUtil.GetValues<Detail>().ToList(),
                Language = Language.English
            });
        }
        
        private static async Task CreateBlob(string containerName, string fileName, string data)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Source"].ToString();
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var client = storageAccount.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = MediaTypes.ApplicationJson;

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                await blob.UploadFromStreamAsync(stream);
            }
        }
    }
}
