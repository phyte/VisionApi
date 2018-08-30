using System.Collections.Generic;
using BMVisionApiClient.VisionAPI.Enum;

namespace BMVisionApiClient.VisionAPI.Models
{
    public class QueryParameters
    {
        public QueryParameters()
        {
            VisualFeatures = new List<VisualFeature>();
            Details = new List<Detail>();
        }
        public List<VisualFeature> VisualFeatures { get; set; }
        public List<Detail> Details { get; set; }
        public string Language { get; set; }
    }
}
