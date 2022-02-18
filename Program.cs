using Exercise1.App.Services;
using Exercise1.DataSource;
using Exercise1.Interfaces;
using Exercise1.Utility;

namespace Excerise1
{
    /// <summary>
    /// Main Executable Method Class.
    /// </summary>
    class Program 
	{
		/// <summary>
		/// Asynchronous programming by creating a method that downloads the resources and aggregates the content length.
		/// </summary>
		/// <returns></returns>
		static async Task Main()
		{
			//Prepare download resources URLs List for testing.
			IEnumerable<string> resourcePaths = DataSourcePath.resourcePaths;

			//Initialize  Cancellation Token Source.
			CancellationTokenSource cancellationTokenSource = new();

			//Get Cancellation Token.
			CancellationToken userCancellationToken = cancellationTokenSource.Token;

			//Provide a way to cancel operation anytime.
			ShowMessage(Constants.cancelMessage);

			//Handle Cancellation by User.
			Task cancelTask = Task.Run(() =>
			{
				//Detect key press by user.
				while (Console.ReadKey().Key != ConsoleKey.Enter)
				{
					//Prompt if any other key is pressed.
					ShowMessage(Constants.wrongCancelKeyMessage);
				}

				//Cancel the Async operation on user request.
				ShowMessage(Constants.validCancelKeyMessage);

				//Cencel async download operation.
				cancellationTokenSource.Cancel();
			});

			//Initialize download service for business operations with specific data repository.
			IDownloadService downloadService = new DownloadService();

			//Download resources asynchronously
			Task aggregateContentLengthsTask = downloadService.DownloadFileAsync(resourcePaths, userCancellationToken);

			//Return task result when completed or cancelled.
			await Task.WhenAny(new[] { cancelTask, aggregateContentLengthsTask });
		}

		/// <summary>
		/// ShowMessage
		/// </summary>
		/// <param name="cancelMessage">Cancel Message</param>
		private static void ShowMessage(string cancelMessage)
        {
			Console.WriteLine(cancelMessage);
		}
	}
}
