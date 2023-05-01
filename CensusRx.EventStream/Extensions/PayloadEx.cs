using System.Diagnostics.Contracts;
using DbgCensus.EventStream.Abstractions.Objects.Events;
using DbgCensus.EventStream.Abstractions.Objects.Events.Worlds;

namespace CensusRx.EventStream;

public static class PayloadEx
{
	[Pure]
	public static ZoneIdentifier GetZoneIdentifier(this IZoneEvent payload) =>
		new(payload.WorldID, payload.ZoneID.Definition);

	public static FacilityIdentifier GetFacilityIdentifier(this IFacilityControl payload) =>
		new FacilityIdentifier(payload.WorldID, payload.ZoneID.Definition, payload.FacilityID);
}