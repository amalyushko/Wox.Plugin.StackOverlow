﻿namespace Wox.Plugin.StackOverlow.Infrascructure.Model
{
    public class SearchRequest
    {
        public string Query { get; set; }

        public int? Page { get; set; }

        public int? PageSize { get; set; }
    }
}