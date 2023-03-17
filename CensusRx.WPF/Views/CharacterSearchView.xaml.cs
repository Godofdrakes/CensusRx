using System.Reactive.Linq;
using CensusRx.WPF.ViewModels;
using ReactiveUI;

namespace CensusRx.WPF.Views;

public partial class CharacterSearchView
{
	public CharacterSearchView()
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{
			this.WhenAnyValue(view => view.FactionInput.SelectedItem)
				.OfType<FactionMatch>()
				.Select(item => item.Match)
				.BindTo(ViewModel, model => model.FactionMatch);
			this.Bind(ViewModel, model => model.Name, view => view.NameInput.Text);
		});
	}
}