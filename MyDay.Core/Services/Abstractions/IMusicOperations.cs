using MyDay.Core.Services.Models.TidalAPI.Playlists;
using MyDay.Core.Services.Models.TidalAPI.Relationships;

namespace MyDay.Core.Services.Abstractions
{
    public interface IMusicOperations
    {
        Task<RelatedPlaylistsResponseWrapper> GetRelatedPlaylists(string keyword);
        Task<PlaylistResponseWrapper> GetPlaylist(string playlistId);
    }
}
