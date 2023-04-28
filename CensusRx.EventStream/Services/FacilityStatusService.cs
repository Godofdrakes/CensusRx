using DynamicData;

namespace CensusRx.EventStream;

public class FacilityStatusService : IFacilityStatusService
{
	public IObservableCache<IFacilityStatusInstance, FacilityIdentifier> FacilityStatus { get; }

	public FacilityStatusService()
	{
		
	}
}