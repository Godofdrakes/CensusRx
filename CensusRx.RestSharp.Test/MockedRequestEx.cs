using RichardSzalay.MockHttp;

namespace CensusRx.RestSharp.Test;

public static class MockedRequestEx
{
	public static MockedRequest Throw(this MockedRequest source, Func<HttpRequestMessage, Exception> exceptionFactory) =>
		source.Respond((Func<HttpRequestMessage, Task<HttpResponseMessage>>)(req =>
			throw exceptionFactory(req)));
}
