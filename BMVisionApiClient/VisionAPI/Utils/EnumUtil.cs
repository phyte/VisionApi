using System.Collections.Generic;
using System.Linq;

namespace BMVisionApiClient.VisionAPI.Utils
{
    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}