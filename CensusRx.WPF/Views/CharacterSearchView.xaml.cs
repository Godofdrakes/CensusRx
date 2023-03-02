using ReactiveUI;

namespace CensusRx.WPF.Views;

public partial class CharacterSearchView
{
	public CharacterSearchView()
	{
		InitializeComponent();
		
		this.WhenActivated(d =>
		{
			this.DataContext = ViewModel;
		});
	}
}