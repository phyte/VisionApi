using System;

namespace BMVisionApiClient.VisionAPI.Exceptions
{
    public class VisionApiClientException : Exception
    {
        public VisionApiClientException(string message) : base(message)
        {
        }
    }
}
