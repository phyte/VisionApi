using System.Collections.Generic;
using System.Linq;
using BMVisionApiClient.VisionAPI.Enum;

namespace BMVisionApiClient.VisionAPI.Extensions
{
    public static class DetailListExtensions
    {
        public static string AggregateDetails(this List<Detail> details)
        {
            var aggregated = details.Aggregate(string.Empty, (current, detail) => current + (detail + ","));
            if (aggregated.Length > 0)
            {
                return aggregated.Remove(aggregated.Length - 1);
            }
            return aggregated;
        }
    }
}
