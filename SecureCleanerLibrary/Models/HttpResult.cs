namespace SecureCleanerLibrary
{
    public class HttpResult
    {
        public HttpResult(string url, string requestBody, string responseBody)
        {
            Url = url;
            RequestBody = requestBody;
            ResponseBody = responseBody;
        }
        public string Url { get; }
        public string RequestBody { get; }
        public string ResponseBody { get; }
    }
}