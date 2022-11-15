using System;
using System.Net.Http.Headers;

namespace HTF2022
{
    internal class HTTPInstance
    {
        private readonly string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMTEiLCJuYmYiOjE2Njg1MDA5MDUsImV4cCI6MTY2ODU4NzMwNSwiaWF0IjoxNjY4NTAwOTA1fQ.FeWVrbAQENxia9_ieBGWAQxqYt-XppbcfUqRuYr90Ysn0_tceWdExlWX4h5M0uFuy3iEs_x49OEDOvJ6FrbIww";

        public HttpClient client;

        public HTTPInstance()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://app-htf-2022.azurewebsites.net")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}