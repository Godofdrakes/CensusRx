using System.Reactive.Concurrency;
using CensusRx.ViewModels;
using Microsoft.Reactive.Testing;
using ReactiveUI;
using ReactiveUI.Testing;
using RichardSzalay.MockHttp;
using TestRx;

namespace CensusRx.RestSharp.Test.ViewModel;

[TestFixture]
public class CensusSearchViewModelTests : CensusTestsBase
{
	public class TestScreen : IScreen
	{
		public RoutingState Router { get; } = new();
	}

	public class TestSearchViewModel : CensusSearchViewModel<Character>
	{
		public TestSearchViewModel(ICensusClient censusClient, IScheduler scheduler)
			: base(new TestScreen(), censusClient, scheduler) { }

		public IObservable<Character> GetCharactersByName(string name) =>
			ExecuteRequest.Execute(request => request
				.Where(character => character.Name.FirstLower)
				.IsEqualTo(name.ToLower()));
	}

	[Test]
	public void PropagatesResults() => new TestScheduler().With(scheduler =>
	{
		MessageHandler.Expect("http://localhost/get/ps2/character")
			.RespondWithJsonFile(CensusJsonData.CHARACTER_LIST);

		var viewModel = new TestSearchViewModel(this.CensusClient, scheduler);

		var viewModelObserver = new TestObserver<Character>
		{
			ValueWriter = WriteCensusObjectId,
			AutoLog = true,
		};

		viewModel.GetCharactersByName("naozumi")
			.Subscribe(viewModelObserver);

		// ReactiveCommand results are scheduled, must pump scheduler
		Assert.DoesNotThrow(scheduler.Start);

		Assert.Multiple(() =>
		{
			Assert.That(viewModel.Errors, Is.Empty);
			Assert.That(viewModel.Results, Is.Not.Empty);
		});

		Assert.That(viewModel.Results.First(), Is.Not.Null);
		Assert.That(viewModel.Results.First().Id, Is.Not.EqualTo(0));
	});

	[Test]
	public void PropagatesErrors() => new TestScheduler().With(scheduler =>
	{
		// register no handlers so the rest request fails

		var viewModel = new TestSearchViewModel(this.CensusClient, scheduler);

		var viewModelObserver = new TestObserver<Character>
		{
			ValueWriter = WriteCensusObjectId,
			AutoLog = true,
		};

		// ReactiveCommand.ThrownExceptions observes exceptions but does not eat them
		// Subscribers of the command execution must still handle (or eat) exceptions
		// ViewModelObserver does that for us here
		viewModel.GetCharactersByName("naozumi")
			.Subscribe(viewModelObserver);

		// ReactiveCommand.ThrownExceptions are scheduled, must pump scheduler
		Assert.DoesNotThrow(scheduler.Start);

		Assert.Multiple(() =>
		{
			Assert.That(viewModel.Errors, Is.Not.Empty);
			Assert.That(viewModel.Results, Is.Empty);
		});
	});
}