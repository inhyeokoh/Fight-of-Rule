using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;

class SheetsToCSV
{
    static readonly string ApiKey = "AIzaSyB56H4golF0m6dUeRXUmGD_V8IAAsIq9QU";  // 발급받은 API 키
    static readonly string SpreadsheetId = "1TZybcbE6EV74T-N9NExXQv5CDpZkE6dHvgCUxOWHyrI";  // 스프레드 시트 ID
    static readonly List<string> SheetNames = new List<string> { "QuestTable", "NpcTable" };  // 시트 이름들


    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            return;
        }

        string outputDirectory = args[0];
        ReadEntries(outputDirectory);
    }

    static void ReadEntries(string outputDirectory)
    {
        SheetsService service = new SheetsService(new BaseClientService.Initializer()
        {
            ApiKey = ApiKey,
            ApplicationName = "FightOfRule",
        });

        foreach (var sheetName in SheetNames)
        {
            var range = $"{sheetName}!A:Z";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;

            if (values != null && values.Count > 0)
            {
                SaveToCSV(sheetName, values, outputDirectory);
            }
            else
            {
                Console.WriteLine($"No data found in sheet: {sheetName}");
            }
        }
    }

    static void SaveToCSV(string sheetName, IList<IList<object>> values, string outputDirectory)
    {
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        string csvPath = Path.Combine(outputDirectory, $"{sheetName}.csv");
        using (StreamWriter writer = new StreamWriter(csvPath))
        {
            foreach (var row in values)
            {
                writer.WriteLine(string.Join(",", row));
            }
        }
        Console.WriteLine($"CSV file saved at: {csvPath}");
    }
}