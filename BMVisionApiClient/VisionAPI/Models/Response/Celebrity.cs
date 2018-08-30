namespace BMVisionApiClient.VisionAPI.Models.Response
{
    public class Celebrity
    {
        public string Name { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
        public double Confidence { get; set; }
    }
}
