using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using CensusRx.WPF.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CensusRx.WPF.Services;

public class JsonOptionsWriter<T> : IOptionsWriter<T>
	where T : class, new()
{
	private readonly IHostEnvironment _environment;
	private readonly IOptionsMonitor<T> _optionsMonitor;
	private readonly string _fileName;
	private readonly string? _section;

	private readonly JsonWriterOptions _writerOptions = new()
	{
		Indented = true,
	};

	public T Value => _optionsMonitor.CurrentValue;

	public JsonOptionsWriter(IHostEnvironment environment, IOptionsMonitor<T> optionsMonitor, string fileName, string? section = default)
	{
		_environment = environment;
		_optionsMonitor = optionsMonitor;
		_fileName = fileName;
		_section = section;
	}

	public T Get(string? name) => _optionsMonitor.Get(name);

	public void Write(Action<T> writeAction)
	{
		var fileProvider = _environment.ContentRootFileProvider;
		var fileInfo = fileProvider.GetFileInfo(_fileName);

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

		var value = _optionsMonitor.CurrentValue;

		writeAction.Invoke(value);

		var node = JsonSerializer.SerializeToNode(value);
		if (node is null)
		{
			throw new InvalidOperationException("failed to serialize value");
		}
		
		if (!string.IsNullOrEmpty(_section))
		{
			json[_section] = node;
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