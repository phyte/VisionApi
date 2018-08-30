using System.Collections.Generic;

namespace BMVisionApiClient.VisionAPI.Models.Response
{
    public class Description
    {
        public Description()
        {
            Tags = new List<string>();
            Captions = new List<Caption>();
        }
        public List<string> Tags { get; set; }
        public List<Caption> Captions { get; set; }
    }
}
