using System;
using System.Net.Http.Headers;

namespace HTF2022
{
    internal class HTTPInstance
    {
        private readonly string token = "";

        public HttpClient client;

        public HTTPInstance()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("")
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}