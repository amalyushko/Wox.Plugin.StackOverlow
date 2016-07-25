﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Infrascructure.Api
{
    public class StackOverflowApi
    {
        private const string API_METHOD_URL = "http://api.stackexchange.com/2.2/search/advanced";

        private const string DEFAULT_ORDER_VALUE = "desc";

        private const string DEFAULT_SITE_VALUE = "stackoverflow";

        private const string DEFAULT_SORT_VALUE = "relevance";

        private const int DEFAULT_PAGE_VALUE = 1;

        private const int DEFAULT_PAGE_SIZE_VALUE = 5;

        private readonly string _apiUrl;

        public StackOverflowApi()
        {
            var defaultParameters = new Dictionary<string, string>
            {
                { SearchParameters.ORDER, DEFAULT_ORDER_VALUE },
                { SearchParameters.SORT, DEFAULT_SORT_VALUE },
                { SearchParameters.SITE, DEFAULT_SITE_VALUE },
            };

            _apiUrl = string.Concat(API_METHOD_URL, ToQueryString(defaultParameters, true));
        }

        public QuestionResponse GetQuestions(SearchRequest request)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var httpClient = new HttpClient(handler))
            {
                var apiUrl = BuildUrl(request);

                httpClient.BaseAddress = new Uri(apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = httpClient.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;

                    var responseString = responseContent.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<QuestionResponse>(responseString);
                }

                return null;
            }
        }

        private string BuildUrl(SearchRequest request)
        {
            var requestParameters = new Dictionary<string, string>
            {
                {SearchParameters.QUESTION, request.Query},
                {SearchParameters.PAGE, request.Page.GetValueOrDefault(DEFAULT_PAGE_VALUE).ToString()},
                {SearchParameters.PAGE_SIZE, request.PageSize.GetValueOrDefault(DEFAULT_PAGE_SIZE_VALUE).ToString()}
            };

            return string.Concat(_apiUrl, ToQueryString(requestParameters));
        }

        private string ToQueryString(IDictionary<string, string> parameters, bool include = false)
        {
            var sb = new StringBuilder();

            if (include)
            {
                sb.Append("?");
            }

            foreach (var parameter in parameters)
            {
                sb.AppendFormat("{0}={1}", HttpUtility.UrlEncode(parameter.Key), HttpUtility.UrlEncode(parameter.Value));
                sb.Append("&");
            }

            return sb.ToString();
        }
    }
}