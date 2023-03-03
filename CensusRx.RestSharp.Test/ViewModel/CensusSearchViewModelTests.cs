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

	private class TestSearchViewModel : CensusSearchViewModel<Character>
	{
		public TestSearchViewModel(ICensusClient censusClient)
			: base(new TestScreen(), censusClient) { }

		public void GetCharactersByName(string name) => ExecuteRequest.Execute(request => request
				.Where(character => character.Name.FirstLower)
				.IsEqualTo(name.ToLower())
				.LimitTo(10))
			.Subscribe();
	}

	[Test]
	public void PropagatesResults() => new TestScheduler().With(scheduler =>
	{
		MessageHandler.Expect("http://localhost/get/ps2/character")
			.RespondWithJsonFile(CensusJsonData.CHARACTER_LIST);

		var viewModel = new TestSearchViewModel(this.CensusClient);

		// ReactiveCommand results are scheduled, must pump scheduler
		Assert.DoesNotThrow(() =>
		{
			scheduler.Schedule(() => viewModel.GetCharactersByName("naozumi"));
			scheduler.Start();
		});

		Assert.Multiple(() =>
		{
			Assert.That(viewModel.Results, Has.Count.EqualTo(4));
		});
	});
}