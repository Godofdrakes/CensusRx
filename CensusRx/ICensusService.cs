namespace CensusRx;

public interface ICensusService
{
	string Endpoint { get; }
	string ServiceId { get; }
	string Namespace { get; }
}
