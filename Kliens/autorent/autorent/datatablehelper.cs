using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace autorent
{
    public class datatablehelper
    {
       public static DataTable? UseSystemTextJson(string sampleJson)
        {
            DataTable? dataTable = new();


            if (string.IsNullOrWhiteSpace(sampleJson))
            {
                return dataTable;
            }


            JsonElement data = JsonSerializer.Deserialize<JsonElement>(sampleJson);
            if (data.ValueKind != JsonValueKind.Array)
            {
                return dataTable;
            }

            var dataArray = data.EnumerateArray();
            JsonElement firstObject = dataArray.First();

            var firstObjectProperties = firstObject.EnumerateObject();
            foreach (var element in firstObjectProperties)
            {
                dataTable.Columns.Add(element.Name);
            }

            foreach (var obj in dataArray)
            {
                var objProperties = obj.EnumerateObject();
                DataRow newRow = dataTable.NewRow();
                foreach (var item in objProperties)
                {
                    newRow[item.Name] = item.Value;
                }
                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }
    }
}
