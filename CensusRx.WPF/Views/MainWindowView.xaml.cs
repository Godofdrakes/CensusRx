using System.Reactive.Linq;
using CensusRx.WPF.ViewModels;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using ReactiveUI;

namespace CensusRx.WPF.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindowView : IWpfShell
	{
		public MainWindowView(MainWindowViewModel viewModel)
		{
			InitializeComponent();

			this.ViewModel = viewModel;

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