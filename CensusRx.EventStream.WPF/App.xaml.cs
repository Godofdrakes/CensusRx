using System.Windows;

namespace CensusRx.EventStream.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			this.MainWindow = new MainWindow()
			{
				ViewModel = new MainWindowViewModel()
				{
					
				}
			};

			this.MainWindow.Show();
		}
	}
}