using MyDay.Core.Services.Models.TidalAPI.Common;

namespace MyDay.Core.Services.Models.TidalAPI.Relationships
{
    public class RelatedPlaylistsResponseWrapper
    {
        public bool IsSuccess { get; set; }
        public RelatedPlaylistsResponseDto RelatedPlaylists { get; set; } = new RelatedPlaylistsResponseDto();
        public TidalAPIErrorResponseDto Error { get; set; } = new TidalAPIErrorResponseDto();
    }
}