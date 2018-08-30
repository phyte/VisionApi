using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BMVisionApiClient.VisionAPI;
using BMVisionApiClient.VisionAPI.Constants;
using BMVisionApiClient.VisionAPI.Enum;
using BMVisionApiClient.VisionAPI.Exceptions;
using BMVisionApiClient.VisionAPI.Models;
using BMVisionApiClient.VisionAPI.Output;
using BMVisionApiClient.VisionAPI.Utils;
using Detail = BMVisionApiClient.VisionAPI.Enum.Detail;
using System.Configuration;

namespace VisionApiTest
{
    class Program
    {
        public static readonly string SubscriptionKey = ConfigurationManager.AppSettings["SubscriptionKey"];

        public const string ImageUrl = "";
        public const string ImagePath = "";

        static void Main()
        {
            Console.Title = "BM Vision API";
            VisionApiClient.SetSubscriptionKey(SubscriptionKey);
            VisionApiClient.SetRegion(AzureRegion.WestEurope);
            VisionApiClient.SetDefaultQueryParameters(new QueryParameters
            {
                VisualFeatures = EnumUtil.GetValues<VisualFeature>().ToList(),
                Details = EnumUtil.GetValues<Detail>().ToList(),
                Language = Language.English
            });

            try
            {
                var task1 = Task.Run(async () => await MakeRequestFromUrl());
                task1.Wait();

                //var task2 = Task.Run(async () => await MakeRequestFromFile());
                //task2.Wait();

                //var task3 = Task.Run(async () => await MakeOCRRequestFromUrl());
                //task3.Wait();

                var task4 = Task.Run(async () => await MakeOCRRequestFromFile());
                task4.Wait();
            }
            catch (VisionApiClientException ex)
            {
                Console.WriteLine("== ERROR ==");
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static async Task MakeRequestFromUrl()
        {
            Console.WriteLine($"Image: {ImageUrl}\n");
            var result = await VisionApiClient.AnalyzeImageUrlAsync(ImageUrl);
            ConsoleWriter.Write(result);
        }

        private static async Task MakeRequestFromFile()
        {
            var image = new FileStream(ImagePath, FileMode.Open);
            var result = await VisionApiClient.AnalyzeImageStreamAsync(image);
            ConsoleWriter.Write(result);
        }

        private static async Task MakeOCRRequestFromUrl()
        {
            Console.WriteLine($"Image: {ImageUrl}\n");
            var result = await VisionApiClient.OCRImageUrlAsync(ImageUrl);
            ConsoleWriter.Write(result);
        }

        private static async Task MakeOCRRequestFromFile()
        {
            var image = new FileStream(ImagePath, FileMode.Open);
            var result = await VisionApiClient.OCRImageStreamAsync(image);
            ConsoleWriter.Write(result);
        }
    }
}
