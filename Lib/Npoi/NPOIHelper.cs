using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Npoi
{
    public class NPOIHelper
    {

        public static void ExportExcel(DataTable dataTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            IRow cells = sheet.CreateRow(0);

            ///设置头
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var column = dataTable.Columns[i];
                cells.CreateCell(i).SetCellValue(column.ColumnName);
            }


        }

        public static void Test()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(String));
            dataTable.Columns.Add("AGE", typeof(Int));
            dataTable.Columns.Add("USERNAME", typeof(Int));
            dataTable.Columns.Add("ADDRESS", typeof(String));
            dataTable.Columns.Add("BIRTHDAY", typeof(String));

            for (int i = 0; i < 10000; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow.ItemArray = new string[] { i.ToString(), "10", "测试" + i, $"山海路{i}号", "2020-01-01" };
                dataTable.Rows.Add(dataRow);
            }

        }

    }
}
