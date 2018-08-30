using System.Collections.Generic;

namespace BMVisionApiClient.VisionAPI.Models.Response.OCR
{
    public class Region
    {
        public Region()
        {
            Lines = new List<Line>();
        }
        public string BoundingBox { get; set; }
        public List<Line> Lines { get; set; }
    }
}