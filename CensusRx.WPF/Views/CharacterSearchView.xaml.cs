using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace CensusRx.WPF.Views;

[ServiceLifetime(ServiceLifetime.Transient)]
public partial class CharacterSearchView
{
	public CharacterSearchView()
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{
			this.Bind(ViewModel, model => model.Name, view => view.NameInput.Text);
		});
	}
}