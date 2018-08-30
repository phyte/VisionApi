namespace BMVisionApiClient.VisionAPI.Models.Response
{
    public class Face
    {
        public int Age { get; set; }
        public string Gender { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
    }
}
