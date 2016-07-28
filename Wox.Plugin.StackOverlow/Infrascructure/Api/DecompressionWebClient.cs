using System;
using System.Net;

namespace Wox.Plugin.StackOverlow.Infrascructure.Api
{
    public class DecompressionWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }
    }
}