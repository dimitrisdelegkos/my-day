namespace MyDay.Core.Application.Models.Music
{
    public class PlaylistModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Songs { get; set; } 
        public string Link { get; set; } = string.Empty;
    }
}