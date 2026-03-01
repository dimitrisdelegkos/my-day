namespace MyDay.API.Models
{
    public class MusicDto
    {
        /// <summary>
        /// A message indicating if any playlist results where found
        /// </summary>
        public string ResultMessage { get; set; } = string.Empty; 
        /// <summary>
        /// A list of the top music playlists matching the incoming criteria
        /// </summary>
        public IEnumerable<PlaylistDto> TopPlaylists { get; set; } = Enumerable.Empty<PlaylistDto>();
    }
}
