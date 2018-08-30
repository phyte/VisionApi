using System.Linq;
using BMVisionApiClient.VisionAPI.Models.Response;

namespace BMVisionApiClient.VisionAPI.Extensions
{
    public static class AnalyzeResponseObjectExtensions
    {
        public static bool HasFaces(this AnalyzeResponseObject responseObject)
        {
            return responseObject.Faces.Any();
        }

        public static bool ContainsCelebrities(this AnalyzeResponseObject responseObject)
        {
            return responseObject.Categories.Any(c => c.Detail != null && c.Detail.Celebrities.Any());
        }

        public static bool ContainsLandmarks(this AnalyzeResponseObject responseObject)
        {
            return responseObject.Categories.Any(c => c.Detail != null && c.Detail.Landmarks.Any());
        }

        public static bool ContainsMales(this AnalyzeResponseObject responseObject)
        {
            return responseObject.Faces.Any(c => c.Gender != null && c.Gender == "Male");
        }

        public static bool ContainsFemales(this AnalyzeResponseObject responseObject)
        {
            return responseObject.Faces.Any(c => c.Gender != null && c.Gender == "Female");
        }

        public static bool ContainsAge(this AnalyzeResponseObject responseObject, int age)
        {
            return responseObject.Faces.Any(c => c.Age == age);
        }

        public static bool ContainsAgeRange(this AnalyzeResponseObject responseObject, int ageFrom, int ageTo)
        {
            if (ageFrom > ageTo)
            {
                var x = ageTo;
                ageTo = ageFrom;
                ageFrom = x;
            }

            return responseObject.Faces.Any(c => c.Age >= ageFrom && c.Age <= ageTo);
        }
    }
}
