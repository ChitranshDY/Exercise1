namespace Exercise1.Interfaces
{
    public interface IDownloadService
    {
        abstract Task DownloadFileAsync(IEnumerable<string> resourcePaths, CancellationToken userCancellationToken);
    }
}
