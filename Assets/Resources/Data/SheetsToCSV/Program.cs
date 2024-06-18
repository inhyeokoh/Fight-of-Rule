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

    static void Main(string[] args)
    {
        ReadEntries();
    }

    static void ReadEntries()
    {
        SheetsService service = new SheetsService(new BaseClientService.Initializer()
        {
            ApiKey = ApiKey,
            ApplicationName = "FightOfRule",
        });

        // 스프레드시트의 모든 시트 이름 가져오기
        SpreadsheetsResource.GetRequest getSpreadsheetRequest = service.Spreadsheets.Get(SpreadsheetId);
        Spreadsheet spreadsheet = getSpreadsheetRequest.Execute();
        List<string> sheetNames = new List<string>();

        foreach (var sheet in spreadsheet.Sheets)
        {
            sheetNames.Add(sheet.Properties.Title);
        }

        foreach (var sheetName in sheetNames)
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
                Console.WriteLine($"No data found in sheet: {sheetName}");
            }
        }
    }

    static void SaveToCSV(string sheetName, IList<IList<object>> values)
    {
        string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TableFiles");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }


        string csvPath = Path.Combine(directoryPath, $"{sheetName}.csv");
        using (StreamWriter writer = new StreamWriter(csvPath))
        {
            foreach (var row in values)
            {
                List<string> quotedRow = new List<string>();
                foreach (var cell in row)
                {
                    string cellValue = cell.ToString();
                    if (cellValue.Contains(","))
                    {
                        cellValue = $"\"{cellValue}\"";  // 쉼표를 포함하는 셀 값을 따옴표로 감싸기
                    }
                    quotedRow.Add(cellValue);
                }
                writer.WriteLine(string.Join(",", quotedRow));
            }
        }
        Console.WriteLine($"CSV file saved at: {csvPath}");
    }
}