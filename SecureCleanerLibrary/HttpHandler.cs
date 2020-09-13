using System.Collections.Generic;

namespace SecureCleanerLibrary
{
    public class HttpHandler
    {
        private HttpResult _currentLog;
        public HttpResult CurrentLog
        {
            get { return _currentLog; }
        }

        public string Process(string url, string body, string response, List<SecureSettings> secures)
        {
            var httpResult = new HttpResult(url, body, response);
            
            ISecureCleaner secureCleaner = new SecureCleaner(secures);
            var clearedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            Log(clearedHttpResult);

            return response;
        }

        protected void Log(HttpResult httpResult)
        {
            _currentLog = new HttpResult(httpResult.Url, httpResult.RequestBody, httpResult.ResponseBody);
        }
    }
}