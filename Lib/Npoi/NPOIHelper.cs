using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ICell = NPOI.SS.UserModel.ICell;

namespace Lib.Npoi
{
    public class ExcelBindSource
    {
        public string KeyName { get; set; }
        public int RowNum { get; set; }

        public int ColNum { get; set; }
    }

    public class NPOIHelper
    {


        private static string GetCellValue(ICell cell)
        {
            string StringData = null;

            switch (cell.CellType)
            {
                case CellType.String: { StringData = cell.StringCellValue; break; }
                case CellType.Numeric: { StringData = cell.NumericCellValue.ToString(); break; }
                case CellType.Unknown: { StringData = cell.StringCellValue.ToString(); break; }
                case CellType.Boolean: { StringData = cell.StringCellValue.ToString(); break; }

            }
            return StringData;
        }


        public static MemoryStream Templte(MemoryStream stream, DataTable table)
        {
            MemoryStream memoryStream = new MemoryStream();

            IWorkbook workbook = new HSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheetAt(0);
            sheet.ForceFormulaRecalculation = true;
            String tableName = table.TableName;

            List<ExcelBindSource> sources = new List<ExcelBindSource>();

            foreach (DataColumn col in table.Columns)
            {
                String dd = "$$" + tableName + "." + col.ColumnName.ToUpper();

                for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);

                    for (int j = row.FirstCellNum; j <= row.LastCellNum; j++)
                    {
                        if (row.GetCell(j) != null && GetCellValue(row.GetCell(j)) != null && dd == GetCellValue(row.GetCell(j)).ToUpper())
                        {
                            sources.Add(new ExcelBindSource { KeyName = col.ColumnName.ToUpper(), RowNum = i, ColNum = j });
                        }
                    }

                }
            }
            if (sources.Count > 0)
            {
                int minrow = sources.Select(a => a.RowNum).OrderBy(a => a).FirstOrDefault();
                int maxrow = sources.Select(a => a.RowNum).OrderByDescending(a => a).FirstOrDefault();

                sheet.ShiftRows(minrow + 1, table.Rows.Count + maxrow, table.Rows.Count - 1);

                foreach (ExcelBindSource source in sources)
                {
                    IRow rowTem = sheet.GetRow(source.RowNum);

                    for (int jj = 0; jj < table.Rows.Count; jj++)
                    {
                        IRow row = sheet.GetRow(source.RowNum + jj);
                        if (row == null) row = sheet.CreateRow(source.RowNum + jj);

                        for (int xx = rowTem.FirstCellNum; xx < rowTem.LastCellNum; xx++)
                        {
                            ICell cell = row.GetCell(xx);
                            if (cell == null)
                                cell = row.CreateCell(xx);
                            if (xx == source.ColNum)
                                cell.SetCellValue(table.Rows[jj][source.KeyName].ToString());
                            cell.CellStyle = sheet.GetRow(source.RowNum).GetCell(xx).CellStyle;
                        }
                    }

                }
            }



            workbook.Write(memoryStream);
            return memoryStream;
        }


        /// <summary>
        /// 未定义列名的时候 默认第一行是列名
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static DataSet ImportExcelToTable(MemoryStream stream, List<String> cols = null)
        {
            DataSet dataSet = new DataSet();


            IWorkbook workbook = new HSSFWorkbook(stream);
            int sheetCount = workbook.NumberOfSheets;
            for (int i = 0; i < sheetCount; i++)
            {
                DataTable dataTable = new DataTable();
                ISheet sheet = workbook.GetSheetAt(i);
                int fristNum = sheet.FirstRowNum;
                if (cols == null)
                {
                    IRow fristRow = sheet.GetRow(sheet.FirstRowNum);

                    foreach (ICell cell in fristRow.Cells)
                    {
                        dataTable.Columns.Add(cell.StringCellValue);
                    }
                    fristNum = fristNum + 1;
                }
                else
                {
                    foreach (string cell in cols)
                    {
                        dataTable.Columns.Add(cell);
                    }
                }

                for (int j = fristNum; j < sheet.LastRowNum; j++)
                {
                    DataRow row = dataTable.NewRow();
                    IRow sheetRow = sheet.GetRow(j);

                    for (int x = 0; x < (cols == null ? sheet.GetRow(sheet.FirstRowNum).Cells.Count : cols.Count); x++)
                    {
                        row[x] = sheetRow.GetCell(x);

                    }
                    dataTable.Rows.Add(row);

                }

                dataSet.Tables.Add(dataTable);
            }
            return dataSet;
        }



        /// <summary>
        /// List Excel 到内存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <param name="cloumnsNames"></param>
        /// <returns></returns>
        public static MemoryStream ListToExcel<T>(List<T> datas, List<String> cloumnsNames)
        {
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            IRow fristRow = sheet.CreateRow(0);

            for (int i = 0; i < cloumnsNames.Count; i++)
            {
                var column = cloumnsNames[i];
                fristRow.CreateCell(i).SetCellValue(column);
            }
            Type type = typeof(T);
            for (int i = 0; i < datas.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                T rowData = datas[i];

                //列名需要和实体字段一样
                //for (int j = 0; j < cloumnsNames.Count; j++)
                //{
                //    PropertyInfo propertie = type.GetProperties().Where(a => a.Name.ToUpper().Equals(cloumnsNames[j].ToString().ToUpper())).FirstOrDefault();

                //    if (propertie != null)
                //        row.CreateCell(j).SetCellValue(propertie.GetValue(rowData) == null ? null : propertie.GetValue(rowData).ToString());
                //}

                PropertyInfo[] properties = type.GetProperties();

                for (int j = 0; j < properties.Length; j++)
                {
                    PropertyInfo propertie = properties[j];

                    if (propertie != null)
                        row.CreateCell(j).SetCellValue(propertie.GetValue(rowData) == null ? null : propertie.GetValue(rowData).ToString());
                }

            }

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }
        /// <summary>
        /// 导出DataTable Excel 到内存中
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
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
                DataRow rowData = dataTable.Rows[i];


                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    row.CreateCell(j).SetCellValue(dataTable.Rows[i][j].ToString());

                }
            }

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }






    }
}
