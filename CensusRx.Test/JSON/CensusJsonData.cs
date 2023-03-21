namespace CensusRx.Test;

public static class CensusJsonData
{
	public static string CharacterList => GetJsonFile("CharacterList.json");
	public static string FactionList => GetJsonFile("FactionList.json");
	public static string WeaponDatasheetList => GetJsonFile("WeaponDatasheetList.json");
	public static string WeaponList => GetJsonFile("WeaponList.json");
	public static string ItemList => GetJsonFile("ItemList.json");
	
	private static string GetJsonFilePath(string file) =>
		Path.Combine(TestContext.CurrentContext.TestDirectory, "JSON", file);

	public static string GetJsonFile(string file)
	{
		var filePath = GetJsonFilePath(file);
		TestContext.WriteLine($"Using JSON data: {file}");
		Assume.That(File.Exists(filePath), () => $"Missing json file: {file}" + Environment.NewLine);
		return File.ReadAllText(filePath);
	}
}
