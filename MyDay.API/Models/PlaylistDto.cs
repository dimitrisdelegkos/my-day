namespace MyDay.API.Models
{
    public class PlaylistDto
    {
        /// <summary>
        /// The title of the music playlist
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// The number of the music tracks that are included in the playlist
        /// </summary>
        public int Tracks { get; set; }
        /// <summary>
        /// The URL of the playlist
        /// </summary>
        public string Url { get; set; } = string.Empty;
    }
}
