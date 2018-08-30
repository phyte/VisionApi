using System.Collections.Generic;

namespace BMVisionApiClient.VisionAPI.Models.Response
{
    public class Detail
    {
        public Detail()
        {
            Celebrities = new List<Celebrity>();
            Landmarks = new List<Landmarks>();
        }
        public List<Celebrity> Celebrities { get; set; }
        public List<Landmarks> Landmarks { get; set; }
    }
}
