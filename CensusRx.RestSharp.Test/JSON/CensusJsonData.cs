using System.Net;
using RichardSzalay.MockHttp;

namespace CensusRx.RestSharp.Test.JSON;

public static class CensusJsonData
{
	public const string MEDIA_TYPE = "application/json";
	public const string CHARACTER = "Character.json";
	public const string CHARACTER_LIST = "CharacterList.json";
	public const string CHARACTER_LIST_EMPTY = "CharacterListEmpty.json";
	public const string FACTION_LIST = "FactionList.json";
	
	private static string GetJsonFilePath(string file) =>
		Path.Combine(TestContext.CurrentContext.TestDirectory, "JSON", file);

	public static string GetJsonFile(string file)
	{
		var filePath = GetJsonFilePath(file);
		TestContext.WriteLine($"Using JSON data: {file}");
		Assume.That(File.Exists(filePath), () => $"Missing json file: {file}" + Environment.NewLine);
		return File.ReadAllText(filePath);
	}

	public static MockedRequest RespondWithJsonFile(this MockedRequest source, string file) =>
		source.Respond(HttpStatusCode.OK, MEDIA_TYPE, GetJsonFile(file));
}
