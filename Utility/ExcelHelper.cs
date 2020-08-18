using System;
using System.Data;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace Demo.Lottery
{
    public static class ExcelHelper
    {
        public static DataSet ReadExcel(string filePath, bool FirstRowIsColumnName)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            FirstRowIsColumnName = true;
            ISheet sheet = null;
            var dataSet = new DataSet();
            IWorkbook workbook = null;
            string sheetName = string.Empty;
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (filePath.EndsWith(".xlsx"))      //2007版本以上
                        workbook = new XSSFWorkbook(fs);
                    else if (filePath.EndsWith(".xls"))  // 2003版本
                        workbook = new HSSFWorkbook(fs);
                }
                var numOfSheets = workbook.NumberOfSheets;
                for (int i = 0; i != numOfSheets; i++)
                {
                    sheet = workbook.GetSheetAt(i);
                    sheetName = sheet.SheetName;
                    if (!string.IsNullOrEmpty(sheetName))
                    {
                        var dt = Read(sheet, sheetName, FirstRowIsColumnName);
                        dataSet.Tables.Add(dt);
                    }
                }
                return dataSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} \r\n{ex.StackTrace}");
                return null;
            }
        }

        private static DataTable Read(ISheet sheet, string sheetName, bool isFirstRowColumn)
        {
            var data = new DataTable();
            int startRow = 0;
            IRow firstRow = sheet.GetRow(startRow);
            if (null == sheet || null == firstRow)
            {
                return data;
            }
            else
            {
                int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                if (isFirstRowColumn)
                {
                    ICell cell = null;
                    int numberOfEmty = 0;
                    for (int i = firstRow.FirstCellNum; i != cellCount; i++)
                    {
                        cell = firstRow.GetCell(i);
                        var cellValue = string.Empty;
                        if (null != cell && !string.IsNullOrEmpty(cell.StringCellValue))
                        {
                            cellValue = cell.StringCellValue;
                        }
                        else
                        {
                            cellValue = $"Column{++numberOfEmty}";
                        }
                        var column = new DataColumn(cellValue);
                        data.Columns.Add(column);
                    }
                    startRow = sheet.FirstRowNum + 1;
                }
                else
                {
                    startRow = sheet.FirstRowNum;
                }

                //最后一行的行号
                int rowCount = sheet.LastRowNum;
                for (int i = startRow; i != rowCount + 1; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (null == row)
                    {
                        continue; //没有数据的行默认是null
                    }

                    DataRow dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j != cellCount; ++j)
                    {
                        var cellValue = row.GetCell(j);
                        if (null != cellValue) //同理，没有数据的单元格都默认是null
                            dataRow[j] = cellValue.ToString();
                    }
                    data.Rows.Add(dataRow);
                }
            }
            RemoveEmptyFromDataTable(ref data);
            data.TableName = sheetName;
            return data;
        }

        private static void RemoveEmptyFromDataTable(ref DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            var columnCount = dt.Columns.Count;
            List<int> listRowDataIndex = new List<int>();
            for (int i = dt.Rows.Count - 1; i != -1; i--)
            {
                var isRowEmpty = true;
                for (int column = 0; column < columnCount; column++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][column].ToString().Replace(" ", string.Empty)))
                    {
                        isRowEmpty = false;
                        break;
                    }
                }
                if (isRowEmpty)
                {
                    listRowDataIndex.Add(i);
                }
            }
            foreach (int i in listRowDataIndex)
            {
                dt.Rows.RemoveAt(i);
            }
        }

        private static IWorkbook CreateWorkbook(string file)
        {
            var folder = Path.GetDirectoryName(file);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var ext = Path.GetExtension(file);
            IWorkbook workbook = null;
            if (ext == ".xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else //ext== ".xls"
            {
                workbook = new HSSFWorkbook();
            }
            return workbook;
        }

        private static void CreateSheetHeader(DataTable table, IRow headerRow)
        {
            foreach (DataColumn column in table.Columns)
            {
                var cell = headerRow.CreateCell(column.Ordinal);
                cell.SetCellValue(column.Caption);
            }
        }

        private static void AddRowData(DataTable table, ISheet sheet)
        {
            //header
            CreateSheetHeader(table, sheet.CreateRow(0));
            IRow excelRow = null;
            DataRow dataRow = null;
            for (int i = 0; i != table.Rows.Count; i++)
            {
                dataRow = table.Rows[i];
                excelRow = sheet.CreateRow(i + 1);
                foreach (DataColumn column in table.Columns)
                {
                    if (typeof(double) == dataRow[column].GetType())
                    {
                        excelRow.CreateCell(column.Ordinal).SetCellValue(Convert.ToDouble(dataRow[column]));
                    }
                    else if (typeof(int) == dataRow[column].GetType())
                    {
                        excelRow.CreateCell(column.Ordinal).SetCellValue(Convert.ToInt32(dataRow[column]));
                    }
                    else if (typeof(long) == dataRow[column].GetType())
                    {
                        excelRow.CreateCell(column.Ordinal).SetCellValue(Convert.ToInt64(dataRow[column]));
                    }
                    else if (typeof(bool) == dataRow[column].GetType())
                    {
                        excelRow.CreateCell(column.Ordinal).SetCellValue(Convert.ToBoolean(dataRow[column]));
                    }
                    else if (typeof(DateTime) == dataRow[column].GetType())
                    {
                        excelRow.CreateCell(column.Ordinal).SetCellValue(Convert.ToDateTime(dataRow[column]).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        excelRow.CreateCell(column.Ordinal).SetCellValue(dataRow[column].ToString());
                    }
                }
            }
            for (int columnIndex = 0; columnIndex != table.Columns.Count; columnIndex++)
            {
                sheet.AutoSizeColumn(columnIndex);
            }
        }

        private static void WriteWithStream(IWorkbook workbook, string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
                fs.Close();
            }
        }

        public static bool ExportExcel(DataTable table, string filePath)
        {
            if (null == table)
            {
                return false;
            }
            try
            {
                IWorkbook workbook = CreateWorkbook(filePath);
                ISheet sheet = workbook.CreateSheet(string.IsNullOrEmpty(table.TableName) ? $"sheet0" : table.TableName);
                AddRowData(table, sheet);

                WriteWithStream(workbook, filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ExportExcel(DataSet data, string filePath)
        {
            if (null == data || 0 == data.Tables.Count)
            {
                return false;
            }
            try
            {
                IWorkbook workbook = CreateWorkbook(filePath);
                ISheet sheet = null;
                DataTable table = null;
                for (int i = 0; i != data.Tables.Count; i++)
                {
                    table = data.Tables[i];
                    sheet = workbook.CreateSheet(string.IsNullOrEmpty(table.TableName) ? $"sheet{i}" : table.TableName);
                    AddRowData(table, sheet);
                }
                WriteWithStream(workbook, filePath);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
