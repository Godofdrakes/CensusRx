using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;

namespace CensusRx.EventStream;

public static class ObservableWebSocket
{
	private static IObservable<T> UsingClientWebSocket<T>(Func<ClientWebSocket, IObservable<T>> observableFactory)
	{
		return Observable.Using(() => new ClientWebSocket(), observableFactory);
	}

	public static IObservable<string> Connect(Uri uri)
	{
		return UsingClientWebSocket(client =>
		{
			return Observable.Create<string>(async (observer, token) =>
			{
				await client.ConnectAsync(uri, token);

				try
				{
					WebSocketReceiveResult result;

					do
					{
						var buffer = new byte[1024];

						result = await client.ReceiveAsync(buffer, token);

						if (result is { MessageType: WebSocketMessageType.Binary })
						{
							throw new NotSupportedException("MessageType: Binary");
						}

						if (result is { EndOfMessage: false })
						{
							throw new NotSupportedException("EndOfMessage: false");
						}

						if (result is { MessageType: WebSocketMessageType.Text, EndOfMessage: true })
						{
							observer.OnNext(Encoding.UTF8.GetString(buffer));
						}
					}
					while (result is not { MessageType: WebSocketMessageType.Close });
				}
				catch (Exception e)
				{
					observer.OnError(e);
				}
				finally
				{
					await client.CloseAsync(WebSocketCloseStatus.NormalClosure, default, token);
				}
			});
		});
	}
}