using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICell = NPOI.SS.UserModel.ICell;

namespace Lib.Npoi
{
    public class NPOIHelper
    {

        public static void ExportExcelTest(DataTable dataTable)
        {

            MemoryStream memoryStream = ExportExcel(dataTable);

            using (FileStream stream = File.OpenWrite(@"D:\mycode\HUBCom\Com\ConsoleApp1\Excel\" + DateTime.Now.Ticks.ToString() + ".xls"))
            {
                byte[] vs = memoryStream.ToArray();
                stream.Write(vs, 0, vs.Length);
            }
        }

        public static MemoryStream ExportExcel(DataTable dataTable)
        {
            IWorkbook workbook = new HSSFWorkbook();
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


                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    row.CreateCell(j).SetCellValue(dataTable.Rows[i][j].ToString());

                }
            }

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;

            //using (FileStream stream = File.OpenWrite(@"D:\mycode\HUBCom\Com\ConsoleApp1\Excel\" + DateTime.Now.Ticks.ToString() + ".xls"))
            //{
            //    workbook.Write(stream);
            //}
        }

        public static void Test()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(String));
            dataTable.Columns.Add("AGE");
            dataTable.Columns.Add("USERNAME");
            dataTable.Columns.Add("ADDRESS", typeof(String));
            dataTable.Columns.Add("BIRTHDAY", typeof(String));

            for (int i = 0; i < 10000; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow.ItemArray = new object[] { i.ToString(), (int)10, "测试" + i, $"山海路{i}号", "2020-01-01" };
                dataTable.Rows.Add(dataRow);
            }

            ExportExcelTest(dataTable);

        }

    }
}
