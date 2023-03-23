using ReactiveUI;

namespace CensusRx.WPF.Views;

public partial class ThemeConfigView
{
	public ThemeConfigView()
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{
			this.OneWayBind(ViewModel, model => model.AllColorSchemes, view => view.ColorScheme.ItemsSource);
			this.OneWayBind(ViewModel, model => model.AllBaseColors, view => view.BaseColor.ItemsSource);
			this.Bind(ViewModel, model => model.ColorScheme, view => view.ColorScheme.SelectedItem);
			this.Bind(ViewModel, model => model.BaseColor, view => view.BaseColor.SelectedItem);
		});
	}
}