using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using CensusRx.WPF.Interfaces;
using Microsoft.Extensions.Hosting;

namespace CensusRx.WPF.Services;

public class JsonOptionsWriter : IOptionsWriter
{
	private readonly IHostEnvironment _environment;

	private readonly JsonWriterOptions _writerOptions = new()
	{
		Indented = true,
	};

	public JsonOptionsWriter(IHostEnvironment environment)
	{
		_environment = environment;
	}

	public void Write<T>(Action<OptionsWriterArguments<T>> writeAction) where T : new()
	{
		var args = new OptionsWriterArguments<T>();

		writeAction.Invoke(args);

		var fileProvider = _environment.ContentRootFileProvider;
		var fileInfo = fileProvider.GetFileInfo(args.File!);

		JsonNode json;

		if (fileInfo.Exists)
		{
			using (var stream = fileInfo.CreateReadStream())
			{
				json = JsonSerializer.Deserialize<JsonObject>(stream)!;
			}
		}
		else
		{
			json = new JsonObject();
		}

		var node = JsonSerializer.SerializeToNode(args.Value);
		if (node is null)
		{
			throw new InvalidOperationException("failed to serialize value");
		}

		if (!string.IsNullOrEmpty(args.Section))
		{
			json[args.Section] = node;
		}
		else
		{
			json = node;
		}

		using (var stream = File.OpenWrite(fileInfo.PhysicalPath!))
		{
			using (var writer = new Utf8JsonWriter(stream, _writerOptions))
			{
				json.WriteTo(writer);
			}
		}
	}
}