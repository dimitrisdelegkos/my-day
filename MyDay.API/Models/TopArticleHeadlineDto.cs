namespace MyDay.API.Models
{
    public class TopArticleHeadlineDto
    {
        /// <summary>
        /// The title of the article
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// The author of the article
        /// </summary>
        public string Author { get; set; } = string.Empty;
        /// <summary>
        /// The URL of the article
        /// </summary>
        public string Url { get; set; } = string.Empty;
        /// <summary>
        /// The source of the article
        /// </summary>
        public string Source { get; set; } = string.Empty;
        /// <summary>
        /// The publish date of the article
        /// </summary>
        public string Date { get; set; } = string.Empty;
    }
}
