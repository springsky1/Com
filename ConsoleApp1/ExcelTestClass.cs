using Lib.Npoi;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ExcelTestClass
    {

        public static void ExportExcelTest(DataTable dataTable)
        {

            MemoryStream memoryStream = NPOIHelper.ExportExcel(dataTable);

            using (FileStream stream = File.OpenWrite(@"D:\mycode\HUBCom\Com\ConsoleApp1\Excel\" + DateTime.Now.Ticks.ToString() + ".xls"))
            {
                byte[] vs = memoryStream.ToArray();
                stream.Write(vs, 0, vs.Length);
            }
            memoryStream.Close();
        }
        public static void ExportExcelTest<T>(List<T> datas, List<String> cloumnsNames)
        {

            MemoryStream memoryStream = NPOIHelper.ListToExcel(datas, cloumnsNames);

            using (FileStream stream = File.OpenWrite(@"D:\mycode\HUBCom\Com\ConsoleApp1\Excel\" + DateTime.Now.Ticks.ToString() + ".xls"))
            {
                byte[] vs = memoryStream.ToArray();
                stream.Write(vs, 0, vs.Length);
            }
            memoryStream.Close();
        }


        public static void ImportExcelTest()
        {
            DataTable dataTable = CreateDataTable();
            //  MemoryStream stream = NPOIHelper.ExportExcel(dataTable);

            byte[] bs = File.ReadAllBytes(@"D:\mycode\HUBCom\Com\ConsoleApp1\Excel\637322401029200684.xls");

            MemoryStream memoryStream = new MemoryStream(bs);


            DataSet data = NPOIHelper.ImportExcelToTable(memoryStream);
        }


        public static void temp()
        {
            byte[] bs = File.ReadAllBytes(@"D:\mycode\HUBCom\Com\ConsoleApp1\Excel\Temp.xls");

            MemoryStream memoryStream = new MemoryStream(bs);

            DataTable table = CreateDataTable();

            MemoryStream data = NPOIHelper.Templte(memoryStream, table);

            using (FileStream stream = File.OpenWrite(@"D:\mycode\HUBCom\Com\ConsoleApp1\Excel\" + DateTime.Now.Ticks.ToString() + "2222.xls"))
            {
                byte[] vs = data.ToArray();
                stream.Write(vs, 0, vs.Length);
            }
        }


        public static void Test()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(String));
            dataTable.Columns.Add("AGE");
            dataTable.Columns.Add("USERNAME");
            dataTable.Columns.Add("ADDRESS", typeof(String));
            dataTable.Columns.Add("BIRTHDAY", typeof(String));


            List<ExcelTest> list = new List<ExcelTest>();

            for (int i = 0; i < 10000; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow.ItemArray = new object[] { i.ToString(), (int)10, "测试" + i, $"山海路{i}号", "2020-01-01" };
                dataTable.Rows.Add(dataRow);

                list.Add(new ExcelTest { ADDRESS = "测试", USERNAME = "测试" + i });
            }

            // ExportExcelTest(dataTable);

            List<String> vs = new List<string>() { "ID", "USERNAME", "ADDRESS", "测试" };

            ExportExcelTest(list, vs);
        }

        private static DataTable CreateDataTable()
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
            return dataTable;
        }

    }

    public class ExcelTest
    {
        public string ID { get; set; }
        public string USERNAME { get; set; }

        public string ADDRESS { get; set; }
    }

}
