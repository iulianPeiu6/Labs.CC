namespace Youtube.Services.Abstracts
{
    public interface IYoutubeService
    {
        Task<string> GetFirstSearchVideoLinkAsync(string searchText);
    }
}