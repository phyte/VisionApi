using System.Collections.Generic;
using System.Linq;
using BMVisionApiClient.VisionAPI.Enum;

namespace BMVisionApiClient.VisionAPI.Extensions
{
    public static class VisualFeatureListExtensions
    {
        public static string AggregateFeatures(this List<VisualFeature> visualFeatures)
        {
            var aggregated = visualFeatures.Aggregate(string.Empty, (current, visualFeature) => current + (visualFeature + ","));
            if (aggregated.Length > 0)
            {
                return aggregated.Remove(aggregated.Length - 1);
            }
            return aggregated;
        }
    }
}
