using System;
using System.Reactive.Linq;
using System.Windows.Controls;
using CensusRx.WPF.ViewModels;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace CensusRx.WPF.Views;

public partial class CharacterSearchView
{
	public CharacterSearchView()
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{
			void SetButtonCommand((TextBox textBox, ReactiveCommand<object, string> command) tuple)
			{
				TextBoxHelper.SetButtonCommand(tuple.textBox, tuple.command);
			}

			this.WhenAnyValue(view => view.NameInput, view => view.ViewModel!.Search)
				.Subscribe(SetButtonCommand);

			this.Bind(ViewModel, model => model.Name, view => view.NameInput.Text);
			this.WhenAnyValue(view => view.FactionInput.SelectedItem)
				.OfType<FactionMatch>()
				.Select(item => item.Match)
				.BindTo(ViewModel, model => model.FactionMatch);
		});
	}
}