namespace BMVisionApiClient.VisionAPI.Models.Response
{
    public class Category
    {
        public Category()
        {
            Detail = new Detail();
        }

        public string Name { get; set; }
        public double Score { get; set; }

        public Detail Detail { get; set; }
    }
}
