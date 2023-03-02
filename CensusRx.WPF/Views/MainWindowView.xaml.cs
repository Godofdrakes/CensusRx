using System.Reactive.Linq;
using CensusRx.WPF.ViewModels;
using ReactiveUI;

namespace CensusRx.WPF.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindowView
	{
		public MainWindowView()
		{
			InitializeComponent();

			this.WhenActivated(d =>
			{
				var selectedMenuItem =
					this.WhenAnyValue(view => view.HamburgerMenu.SelectedItem)
						.DistinctUntilChanged()
						.Cast<CensusMenuItem>();

				selectedMenuItem
					.Select(item => item?.ViewModel)
					.WhereNotNull()
					.InvokeCommand(ViewModel, model => model.ResetViewModel);

				this.HamburgerMenu.SelectedIndex = 0;
			});
		}
	}
}