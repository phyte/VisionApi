using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BMVisionApiClient.VisionAPI.Constants;
using BMVisionApiClient.VisionAPI.Enum;
using BMVisionApiClient.VisionAPI.Exceptions;
using BMVisionApiClient.VisionAPI.Extensions;
using BMVisionApiClient.VisionAPI.Models;
using BMVisionApiClient.VisionAPI.Models.Request;
using BMVisionApiClient.VisionAPI.Models.Response;
using BMVisionApiClient.VisionAPI.Models.Response.Error;
using BMVisionApiClient.VisionAPI.Models.Response.OCR;
using Newtonsoft.Json;

namespace BMVisionApiClient.VisionAPI
{
    public static class VisionApiClient
    {
        private static string _subscriptionKey = string.Empty;
        private static string _region = AzureRegion.WestEurope;
        private static readonly string DefaultLanguage = Language.English;
        private static QueryParameters _queryParameters = new QueryParameters
        {
            VisualFeatures = new List<VisualFeature> { VisualFeature.Categories },
            Details = new List<Enum.Detail>(),
            Language = DefaultLanguage
        };

        private static bool IsValidLanguage(string language)
        {
            return !string.IsNullOrEmpty(language) && language == Language.English || language == Language.Chinese;
        }

        public static void SetSubscriptionKey(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public static void SetRegion(string region)
        {
            _region = region;
        }

        private static void VerifySubscriptionKey()
        {
            if (string.IsNullOrEmpty(_subscriptionKey))
                throw new MissingSubscriptionKeyException();
        }

        public static void SetDefaultQueryParameters(QueryParameters queryParameters)
        {
            if (!queryParameters.VisualFeatures.Any())
            {
                queryParameters.VisualFeatures.Add(VisualFeature.Categories);
            }
            _queryParameters = queryParameters;
            _queryParameters.Language = IsValidLanguage(queryParameters.Language) ? queryParameters.Language : DefaultLanguage;
        }

        // Supported image formats: JPEG, PNG, GIF, BMP.
        // Image file size must be less than 4MB.
        // Image dimensions must be at least 50 x 50.
        public static async Task<AnalyzeResponseObject> AnalyzeImageUrlAsync(string imageUrl, List<VisualFeature> visualFeatures = null, List<Enum.Detail> details = null, string language = null)
        {
            VerifySubscriptionKey();

            var apiBaseUrl = $"https://{_region}.api.cognitive.microsoft.com";

            var queryParameters = new QueryParameters
            {
                VisualFeatures = visualFeatures != null && !visualFeatures.Any()
                    ? visualFeatures
                    : _queryParameters.VisualFeatures,
                Details = details != null && !details.Any() ? details : _queryParameters.Details,
                Language = IsValidLanguage(language) ? language : _queryParameters.Language
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var uri = $"{apiBaseUrl}/vision/v1.0/analyze?visualFeatures={queryParameters.VisualFeatures.AggregateFeatures()}&details={queryParameters.Details.AggregateDetails()}&language={queryParameters.Language}";

            var csro = new RequestObject { Url = imageUrl };

            var body = JsonConvert.SerializeObject(csro);

            var byteData = Encoding.UTF8.GetBytes(body);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypes.ApplicationJson);
                var response = await client.PostAsync(uri, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<AnalyzeResponseObject>(responseBody);
                }
                var error = JsonConvert.DeserializeObject<Error>(responseBody);
                throw new VisionApiClientException(error.Message);
            }
        }

        // Supported image formats: JPEG, PNG, GIF, BMP.
        // Image file size must be less than 4MB.
        // Image dimensions must be at least 50 x 50.
        public static async Task<AnalyzeResponseObject> AnalyzeImageStreamAsync(Stream stream, List<VisualFeature> visualFeatures = null, List<Enum.Detail> details = null, string language = null)
        {
            VerifySubscriptionKey();

            var apiBaseUrl = $"https://{_region}.api.cognitive.microsoft.com";

            var queryParameters = new QueryParameters
            {
                VisualFeatures = visualFeatures != null && !visualFeatures.Any()
                    ? visualFeatures
                    : _queryParameters.VisualFeatures,
                Details = details != null && !details.Any() ? details : _queryParameters.Details,
                Language = IsValidLanguage(language) ? language : _queryParameters.Language
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var uri = $"{apiBaseUrl}/vision/v1.0/analyze?visualFeatures={queryParameters.VisualFeatures.AggregateFeatures()}&details={queryParameters.Details.AggregateDetails()}&language={queryParameters.Language}";
            
            var byteData = ReadFully(stream);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypes.ApplicationOctetStream);
                var response = await client.PostAsync(uri, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<AnalyzeResponseObject>(responseBody);
                }
                var error = JsonConvert.DeserializeObject<Error>(responseBody);
                throw new VisionApiClientException(error.Message);
            }
        }

        // Supported image formats: JPEG, PNG, GIF, BMP.
        // Image file size must be less than 4MB.
        // Image dimensions must be at least 50 x 50.
        public static async Task<OCRResponseObject> OCRImageStreamAsync(Stream stream)
        {
            VerifySubscriptionKey();
            var apiBaseUrl = $"https://{_region}.api.cognitive.microsoft.com";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var uri = $"{apiBaseUrl}/vision/v1.0/ocr?language=unk&detectOrientation=true";

            var byteData = ReadFully(stream);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypes.ApplicationOctetStream);
                var response = await client.PostAsync(uri, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<OCRResponseObject>(responseBody);
                }
                var error = JsonConvert.DeserializeObject<Error>(responseBody);
                throw new VisionApiClientException(error.Message);
            }
        }

        // Supported image formats: JPEG, PNG, GIF, BMP.
        // Image file size must be less than 4MB.
        // Image dimensions must be at least 50 x 50.
        public static async Task<OCRResponseObject> OCRImageUrlAsync(string imageUrl)
        {
            VerifySubscriptionKey();

            var apiBaseUrl = $"https://{_region}.api.cognitive.microsoft.com";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var uri = $"{apiBaseUrl}/vision/v1.0/ocr?language=unk&detectOrientation=true";

            var csro = new RequestObject { Url = imageUrl };

            var body = JsonConvert.SerializeObject(csro);

            var byteData = Encoding.UTF8.GetBytes(body);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypes.ApplicationJson);
                var response = await client.PostAsync(uri, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<OCRResponseObject>(responseBody);
                }
                var error = JsonConvert.DeserializeObject<Error>(responseBody);
                throw new VisionApiClientException(error.Message);
            }
        }


        private static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
