using System.Windows;
using System.Windows.Markup;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;

namespace CensusRx.WPF.Common;

public class WpfAppInitService : IWpfService
{
	public void Initialize(Application application)
	{
		if (application is IComponentConnector app)
		{
			app.InitializeComponent();
		}
	}
}