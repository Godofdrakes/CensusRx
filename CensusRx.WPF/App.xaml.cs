using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using CensusRx.Interfaces;
using CensusRx.Model;
using CensusRx.WPF.Interfaces;
using CensusRx.WPF.ServiceModules;
using CensusRx.WPF.ViewModels;
using CensusRx.WPF.Views;
using ReactiveUI;
using Splat;

using Expression = System.Linq.Expressions.Expression;

namespace CensusRx.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	private static Func<object> TypeFactory(TypeInfo typeInfo)
	{
		var parameterlessConstructor = typeInfo.DeclaredConstructors.FirstOrDefault(ci => ci.IsPublic && ci.GetParameters().Length == 0);
		if (parameterlessConstructor is null)
		{
			throw new Exception($"Failed to register type {typeInfo.FullName} because it's missing a parameterless constructor.");
		}

		return Expression.Lambda<Func<object>>(Expression.New(parameterlessConstructor)).Compile();
	}

	private List<IServiceModule> LoadedModules { get; } = new();

	private void App_OnStartup(object sender, StartupEventArgs e)
	{
		foreach (var type in Assembly.GetExecutingAssembly().DefinedTypes
			         .Where(type => type is { IsClass: true, IsAbstract: false })
			         .Where(type => type.ImplementedInterfaces.Contains(typeof(IServiceModule))))
		{
			Debug.WriteLine($"Loading service module: {type.FullName}");

			var module = (IServiceModule)TypeFactory(type).Invoke();
			if (module is null)
			{
				throw new Exception();
			}

			LoadedModules.Add(module);
		}

		Locator.CurrentMutable.RegisterLazySingleton<ICensusCache<FactionViewModel>>(() =>
		{
			var cache = new CensusCache<FactionViewModel>(10);
			var client = Locator.Current.GetServiceChecked<ICensusClient>();
			client.Get<Faction>(request => request.LimitTo(10))
				.SelectMany(json => json.UnwrapCensusCollection<Faction>())
				.Select(faction => new FactionViewModel(faction))
				.Subscribe(cache.Precache);
			return cache;
		});

		Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());

		foreach (var module in LoadedModules)
		{
			Debug.WriteLine($"Registering service module: {module.GetType().FullName}");
			module.Register(Locator.CurrentMutable);
		}

		foreach (var module in LoadedModules)
		{
			Debug.WriteLine($"Starting service module: {module.GetType().FullName}");
			module.Startup(Locator.Current);
		}

		var mainWindow = new MainWindowView
		{
			ViewModel = new MainWindowViewModel
			{
				Title = "CensusRx.WPF",
			},
		};

		this.MainWindow = mainWindow;
		this.MainWindow.Show();
	}
}