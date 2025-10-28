using EPiServer.Shell.Navigation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace PluginWithServerSentEvents.Business.AdminTools.Counting
{
	[MenuItem(MenuPaths.Global + "/cms/admintools", Text = "Importer")]
	[Route("/episerver/admintools/importer")]
	public class CustomFileImportController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<ServerSentEventsResult<SseMessage>> Index(string action, CancellationToken cancellationToken)
		{
			return TypedResults.ServerSentEvents(DoImport(cancellationToken), eventType: nameof(SseMessage));
		}

		private async IAsyncEnumerable<SseMessage> DoImport([EnumeratorCancellation] CancellationToken cancellationToken)
		{
			yield return new SseMessage { Message = "Retrieving tasks..." };
			var tasks = GetTasks();
			
			for (int i = 0; i < tasks.Count; i++)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					yield return new SseMessage { Message = "Import aborted" };
					yield break;
				}

				var task = tasks[i];
				yield return new SseMessage { Message = $"Processing task {i+1}/{tasks.Count}: {task}... " };
				await ProcessAsync(task, cancellationToken);
			}

			yield return new SseMessage { Message = "Import finished" };
		}

		private List<string> GetTasks()
		{
			return [
				"User accounts",
				"Order history",
				"Transaction history",
				"Reseller information",
				"New products"
			];
		}

		private async Task<bool> ProcessAsync(string line, CancellationToken cancellationToken)
		{
			// fake doing some work
			await Task.Delay(1000, cancellationToken);
			return true;
		}
	}
}
