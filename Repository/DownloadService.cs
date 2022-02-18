using Exercise1.Interfaces;
using System.Diagnostics;

namespace Exercise1.App.Services
{
    public class DownloadService: IDownloadService
	{
		/// <summary>
		/// Asynchronous method that downloads File & aggregates the content length of all responses & The caller should be able to cancel the operation at any time. 
		/// </summary>
		/// <param name="resourcePaths">Pass the URLs list.</param>
		/// <param name="userCancellationToken">Pass the cancellation token.</param>
		/// <returns></returns>
		public async Task DownloadFileAsync(IEnumerable<string> resourcePaths, CancellationToken requestCancellationToken)
		{
			//Initialize httpClient for request.
			HttpClient httpClient = new();

			//Prepare contents list to show the results.
			List<int> contentLengths = new();

			//Initialize Stopwatch to determine total time.
			var stopwatch = Stopwatch.StartNew();

			//Process for each URL asynchronously
			foreach (string url in resourcePaths)
			{
				//Get content lengh for a specific URL.
				int contentLength = await DownloadUrlContentAsync(url, httpClient, requestCancellationToken);

				//Store results to create maximum possible result before cancelation.
				contentLengths.Add(contentLength);
			}

			//Stop watch after Async operation completed.
			stopwatch.Stop();

			//Print total results on the screen.
			var totalLength = contentLengths.Sum();
			ShowMessage($"\nTotal Length:  {totalLength}");
		}

		/// <summary>
		/// Method to process each URL seperate asynchronously.
		/// </summary>
		/// <param name="resourceURL"></param>
		/// <param name="httpClient"></param>
		/// <param name="userCancellationToken"></param>
		/// <returns>Content length for a specific URL.</returns>
		public static async Task<int> DownloadUrlContentAsync(string resourceURL, HttpClient httpClient, CancellationToken userCancellationToken)
		{
			//Get HttpResponseMessage URL content.
			HttpResponseMessage httpResponse = await httpClient.GetAsync(resourceURL, userCancellationToken);

			//ReadAsByteArrayAsync.
			byte[] contentBytesArray = await httpResponse.Content.ReadAsByteArrayAsync(userCancellationToken);

			//Determine length and print intermediate results on the screen.
			ShowMessage($"{resourceURL,-60} {contentBytesArray.Length,10:#,#}");

			//Return specific URL content length to the calling async method.
			return contentBytesArray.Length;
		}

		/// <summary>
		/// Display Message
		/// </summary>
		/// <param name="message"></param>
		public static void ShowMessage(string message)
		{
			Console.WriteLine(message);
		}
	}
}
