using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICell = NPOI.SS.UserModel.ICell;

namespace Lib.Npoi
{
    public class NPOIHelper
    {

        public static void ExportExcel(DataTable dataTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            IRow fristRow = sheet.CreateRow(0);

            ///设置头
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var column = dataTable.Columns[i];
                fristRow.CreateCell(i).SetCellValue(column.ColumnName);
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                DataRow rowData = dataTable.Rows[0];

                // ICell cell = row.Cells.Add("");

                //List<ICell> cells1 = row.Cells.AddRange((IEnumerable<ICell>)rowData.ItemArray);//.AsQueryable().Select(o => new ICell { cells2.Cells. } o.ToString())

                List<ICell> cells1 = ((List<ICell>)(IEnumerable<ICell>)rowData.ItemArray);//.AsQueryable().Select(o => new ICell { cells2.Cells. } o.ToString())

                //  row.Cells.AddRange(cells1);
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
                dataRow.ItemArray = new object[] { i.ToString(), (int)10, "测试" + i, $"山海路{i}号", "2020-01-01" };
                dataTable.Rows.Add(dataRow);
            }

        }

    }
}
