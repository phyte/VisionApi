using System.Collections.Generic;

namespace BMVisionApiClient.VisionAPI.Models.Response.OCR
{
    public class OCRResponseObject
    {
        public OCRResponseObject()
        {
            Regions = new List<Region>();
        }

        public string Language { get; set; }
        public double TextAngle { get; set; }
        public string Orientation { get; set; }
        public List<Region> Regions { get; set; }
    }
}
