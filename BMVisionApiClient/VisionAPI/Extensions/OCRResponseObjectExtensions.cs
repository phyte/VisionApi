using System.Linq;
using BMVisionApiClient.VisionAPI.Models.Response.OCR;

namespace BMVisionApiClient.VisionAPI.Extensions
{
    public static class OCRResponseObjectExtensions
    {
        public static bool HasWords(this OCRResponseObject responseObject)
        {
            return responseObject.Regions.Any(r => r.Lines.Any(l => l.Words.Any()));
        }
    }
}
