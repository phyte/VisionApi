﻿namespace BMVisionApiClient.VisionAPI.Models.Response
{
    public class Adult
    {
        public bool IsAdultContent { get; set; }
        public bool IsRacyContent { get; set; }
        public double AdultScore { get; set; }
        public double RacyScore { get; set; }
    }
}
