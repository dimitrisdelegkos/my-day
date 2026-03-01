using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDay.Integrations.Application.Models.NewsAPI
{
    public class NewsArticleDto
    {
        public SourceDto Source { get; set; } = new SourceDto();
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string UrlToImage { get; set; } = string.Empty;
        public string PublishedAt { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
} 