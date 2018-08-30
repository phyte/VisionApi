using System.Collections.Generic;

namespace BMVisionApiClient.VisionAPI.Models.Response.OCR
{
    public class Line
    {
        public Line()
        {
            Words = new List<Word>();
        }
        public string BoundingBox { get; set; }
        public List<Word> Words { get; set; }
    }
}