using System.Collections.Generic;

namespace BMVisionApiClient.VisionAPI.Models.Response
{
    public class Color
    {
        public Color()
        {
            DominantColors = new List<string>();
        }

        public string DominantColorForeground { get; set; }
        public string DominantColorBackground { get; set; }
        public List<string> DominantColors { get; set; }
        public string AccentColor { get; set; }
        public bool IsBWImg { get; set; }
    }

}
