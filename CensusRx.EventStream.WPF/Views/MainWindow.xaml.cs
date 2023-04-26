using System.Reactive.Linq;
using System.Windows.Data;
using CensusRx.WPF.Common;
using ReactiveUI;

namespace CensusRx.EventStream.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
	public MainWindow()
	{
		InitializeComponent();

		if (this.DataGridStatusColumn.Binding is Binding binding)
		{
			binding.Converter = OneWayValueConverter.Create(isOnline =>
				isOnline is true ? "Online" : "Offline");
		}
	}
}