namespace CensusRx.EventStream.WPF.DesignData;

public class NullMainWindowViewModel : MainWindowViewModel
{
	public NullMainWindowViewModel() : base(IWorldStatusService.Null) { }
}