using System.Reactive.Linq;
using CensusRx.WPF.ViewModels;
using ReactiveUI;

namespace CensusRx.WPF.Views;

public partial class WeaponSearchView
{
	public WeaponSearchView()
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{
			this.Bind(ViewModel, model => model.Name, view => view.NameInput.Text);
			this.WhenAnyValue(view => view.FactionInput.SelectedItem)
				.OfType<FactionMatch>()
				.Select(item => item.Match)
				.BindTo(ViewModel, model => model.FactionMatch);
		});
	}
}