using System.Reactive.Concurrency;
using System.Reactive.Linq;
using CensusRx.RestSharp.Test.JSON;
using CensusRx.ViewModels;
using Microsoft.Reactive.Testing;
using ReactiveUI;
using ReactiveUI.Testing;
using RichardSzalay.MockHttp;

namespace CensusRx.RestSharp.Test.ViewModel;

[TestFixture]
public class CensusSearchViewModelTests : CensusTestsBase
{
	private class TestScreen : IScreen
	{
		public RoutingState Router { get; } = new();
	}

	[Test]
	public void PropagatesResults() => new TestScheduler().With(scheduler =>
	{
		MessageHandler.Expect("http://localhost/get/ps2/character")
			.RespondWithJsonFile(CensusJsonData.CHARACTER_LIST);

		var viewModel = new CharacterSearchViewModel(new TestScreen(), CensusClient);

		// ReactiveCommand results are scheduled, must pump scheduler
		Assert.DoesNotThrow(() =>
		{
			scheduler.Schedule(() => viewModel.NameSearch.Execute("naozumi").Subscribe());
			scheduler.Start();
		});

		Assert.Multiple(() =>
		{
			Assert.That(viewModel.Results, Has.Count.EqualTo(4));
		});
	});
}