using MyDay.Core.Services.Models.TidalAPI.Common;

namespace MyDay.Core.Services.Models.TidalAPI.Playlists
{
    public class PlaylistResponseWrapper
    {
        public bool IsSuccess { get; set; }
        public PlaylistResponseDto Playlist { get; set; } = new PlaylistResponseDto();
        public TidalAPIErrorResponseDto Error { get; set; } = new TidalAPIErrorResponseDto();
    }
}