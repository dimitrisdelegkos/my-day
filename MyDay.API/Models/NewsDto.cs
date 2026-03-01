namespace MyDay.API.Models
{
    public class NewsDto
    {
        /// <summary>
        /// A message indicating if any news results where found
        /// </summary>
        public string ResultMessage { get; set; } = string.Empty; 
        /// <summary>
        /// A list of the top headlines matching the incoming criteria
        /// </summary>
        public IEnumerable<TopArticleHeadlineDto> TopArticleHeadlines { get; set; } = Enumerable.Empty<TopArticleHeadlineDto>();
    }
}
