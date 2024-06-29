/*using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GoogleSheetsToCSV : MonoBehaviour
{
    static readonly string ApiKey = "AIzaSyB56H4golF0m6dUeRXUmGD_V8IAAsIq9QU";  // 발급받은 API 키
    static readonly string SpreadsheetId = "1TZybcbE6EV74T-N9NExXQv5CDpZkE6dHvgCUxOWHyrI";  // 스프레드 시트 ID
    static readonly List<string> SheetNames = new List<string> { "QuestTable", "NpcTable" };  // 시트 이름들

    void Start()
    {
        ReadEntries();
    }

    void ReadEntries()
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
                SaveToCSV(sheetName, values);
            }
            else
            {
                Debug.Log($"No data found in sheet: {sheetName}");
            }
        }
    }

    void SaveToCSV(string sheetName, IList<IList<object>> values)
    {
        string directoryPath = Path.Combine(Application.dataPath, "Resources/Data");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string csvPath = Path.Combine(directoryPath, $"{sheetName}.csv");
        using (StreamWriter writer = new StreamWriter(csvPath))
        {
            foreach (var row in values)
            {
                writer.WriteLine(string.Join(",", row));
            }
        }
        Debug.Log($"CSV file saved at: {csvPath}");
    }
}
*/