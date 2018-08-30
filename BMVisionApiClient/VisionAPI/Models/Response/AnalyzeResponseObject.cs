using System.Collections.Generic;

namespace BMVisionApiClient.VisionAPI.Models.Response
{
    public class AnalyzeResponseObject
    {
        public AnalyzeResponseObject()
        {
            Categories = new List<Category>();
            Tags = new List<Tag>();
            Faces = new List<Face>();
        }

        public List<Category> Categories { get; set; }

        public Adult Adult { get; set; }

        public List<Tag> Tags { get; set; }

        public Description Description { get; set; }

        public string RequestId { get; set; }

        public Metadata Metadata { get; set; }

        public List<Face> Faces { get; set; }

        public Color Color { get; set; }

        public ImageType ImageType { get; set; }
    }
}
