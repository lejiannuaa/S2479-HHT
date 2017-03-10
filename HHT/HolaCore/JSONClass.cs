using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace HolaCore
{
    public class JSONClass
    {
        public static string DataTableToString(DataTable[] dts)
        {
            StringBuilder JsonString = new StringBuilder();
            JsonString.Append("{\"root\":");

            if (dts != null && dts.Length > 0)
            {
                JsonString.Append("{");

                for (int n = 0; n < dts.Length; n++)
                {
                    DataTable dt = dts[n];
                    JsonString.Append("\"" + dt.TableName + "\":");

                    JsonString.Append("[");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        JsonString.Append("{");

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            JsonString.Append( "\"" + dt.Columns[j].ColumnName + "\":\"" + dt.Rows[i][j].ToString() + "\"");

                            if (j < dt.Columns.Count - 1)
                                JsonString.Append(",");
                        }

                        JsonString.Append("}");

                        if (i < dt.Rows.Count - 1)
                            JsonString.Append(",");
                    }

                    JsonString.Append("]");

                    if (n < dts.Length - 1)
                        JsonString.Append(",");
                }

                JsonString.Append("}");
            }

            JsonString.Append("}");

            return JsonString.ToString();
        }
    }
}
