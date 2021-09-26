using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ExcelDataReader;
using GProof.Alerta.ExcelHelper.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

namespace GProof.Alerta.ExcelHelper
{
    public static class ExcelHelper
    {
        public static void ReplaceHeaders(string filePath, List<string> headers)
        {
            ExcelPackage excelPackage = ReadExcelPackage(filePath);
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

            int col = 1;
            foreach (var header in headers)
            {
                worksheet.Cells[1, col++].Value = header;
            }
            SaveExcelFile(excelPackage);
        }

        private static DataSet OpenExcelStream(string fileName, Stream stream, bool useHeader, bool useEncoding = true)
        {
            if (useEncoding)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            }

            IExcelDataReader excelReader;
            if (Path.GetExtension(fileName).ToUpper() == ".CSV" || Path.GetExtension(fileName).ToUpper() == ".TXT")
            {
                excelReader = ExcelReaderFactory.CreateCsvReader(stream);
            }
            else if (Path.GetExtension(fileName).ToUpper() == ".XLS")
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            var dataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = useHeader
                }
            });

            return dataSet;
        }

        public static DataSet OpenExcelStream(DataFile dataFile, bool useHeader, bool useEncoding = true)
        {
            using var stream = dataFile.StreamFactory.Invoke();
            return OpenExcelStream(fileName: dataFile.Filename,
                stream: stream,
                useHeader: useHeader,
                useEncoding: useEncoding);
        }

        public static DataSet OpenExcelFile(string filePath, byte[] content, bool useHeader, bool useEncoding = true)
            => OpenExcelStream(FileHelper.CreateDataFile(filePath, content), useHeader, useEncoding);


        public static DataSet OpenExcelFile(string filePath, bool useHeader, bool useEncoding = true)
            => OpenExcelStream(FileHelper.CreateDataFile(filePath), useHeader, useEncoding);

        public static List<T> ReadExcelWorksheet<T>(string filePath, string worksheetName = null, int skip = 0) where T : class, new()
        {
            DataSet result = OpenExcelFile(filePath, useHeader: true);
            DataTable dataTable = string.IsNullOrEmpty(worksheetName) ? result.Tables[0] : result.Tables[worksheetName];
            return dataTable.DataTableToList<T>(skip);
        }

        public static List<T> ReadExcelWorksheet<T>(InputFile inputFile, string worksheetName = null, int skip = 0) where T : class, new()
        {
            //string fullPath = Path.Combine(inputFile.FilePath, inputFile.FileName);
            DataSet result = inputFile.Content == null
                ? OpenExcelFile(inputFile.FilePath, useHeader: true)
                : OpenExcelFile(inputFile.FileName, inputFile.Content, true);

            DataTable dataTable = string.IsNullOrEmpty(worksheetName) ? result.Tables[0] : result.Tables[worksheetName];
            return dataTable.DataTableToList<T>(skip);
        }

        public static List<T> ReadExcelWorksheet<T>(string filePath, byte[] content, string worksheetName = null, int skip = 0, bool useHeader = true) where T : class, new()
        {
            DataSet result = OpenExcelFile(filePath, content, useHeader: useHeader);
            DataTable dataTable = string.IsNullOrEmpty(worksheetName) ? result.Tables[0] : result.Tables[worksheetName];
            return dataTable.DataTableToList<T>(skip);
        }

        public static List<T> ReadExcelWorksheet<T>(DataFile dataFile, string worksheetName = null, int skip = 0) where T : class, new()
        {
            DataSet result = OpenExcelStream(dataFile, true);
            DataTable dataTable = string.IsNullOrEmpty(worksheetName)
                ? result.Tables[0]
                : result.Tables[worksheetName];
            return dataTable.DataTableToList<T>(skip);
        }

        public static List<T> ReadExcelWorksheetReplaceHeadersIfNeeded<T>(string filePath,
            string worksheetName = null, int skip = 0) where T : class, new()
        {
            List<T> excelRows = ReadExcelWorksheet<T>(filePath, worksheetName, skip);
            if (!excelRows.All(IsAllNullOrEmpty))
            {
                return excelRows;
            }

            ReplaceHeaders(filePath, new T().GetType().GetProperties().Select(prop => prop.Name).ToList());
            excelRows = ReadExcelWorksheet<T>(filePath, worksheetName, skip);
            return excelRows;
        }

        public static List<T> ReadExcelWorksheetWithColumnMappings<T>(DataFile dataFile, string worksheetName = null) where T : class, new()
        {
            DataSet result = OpenExcelStream(dataFile, useHeader: true);
            DataTable dataTable = string.IsNullOrEmpty(worksheetName) ? result.Tables[0] : result.Tables[worksheetName];
            return dataTable.DataTableToList<T>(GetHeadersToPropertiesMapping(typeof(T)));
        }

        public static List<T> ReadExcelWorksheetWithColumnMappings<T>(string fileName, Stream stream, string worksheetName = null) where T : class, new()
        {
            DataSet result = OpenExcelStream(fileName, stream, useHeader: true);
            DataTable dataTable = string.IsNullOrEmpty(worksheetName) ? result.Tables[0] : result.Tables[worksheetName];
            return dataTable.DataTableToList<T>(GetHeadersToPropertiesMapping(typeof(T)));
        }

        public static List<T> ReadExcelWorksheetWithColumnMappings<T>(string filePath, string worksheetName = null) where T : class, new()
        {
            DataSet result = OpenExcelFile(filePath, useHeader: true);
            DataTable dataTable = string.IsNullOrEmpty(worksheetName) ? result.Tables[0] : result.Tables[worksheetName];
            return dataTable.DataTableToList<T>(GetHeadersToPropertiesMapping(typeof(T)));
        }

        public static ExcelPackage ReadExcelPackage(string filePath)
        {
            return new ExcelPackage(new FileInfo(filePath)); ;
        }

        public static ExcelPackage ReadExcelPackage(byte[] fileAsByteArray)
        {
            using MemoryStream memStream = new MemoryStream(fileAsByteArray);
            ExcelPackage package = new ExcelPackage(memStream);
            return package;
        }

        public static List<T> ReadExcelWorksheetWithoutHeaders<T>(string filePath, string worksheetName = null) where T : class, new()
        {
            List<T> result = ReadExcelWorksheet<T>(filePath, worksheetName, 1);
            return result;
        }

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table, int skip = 0) where T : class, new()
        {
            if (table == null)
            {
                return new List<T>();
            }

            Dictionary<string, string> headersToProperties = GetHeadersToPropertiesMapping(typeof(T));

            PropertyInfo[] propertyInfos =
                typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy
                                        | BindingFlags.Public)
                    .Where(prop => prop.SetMethod != null)
                    .ToArray();

            var tableColumnMap = table.Columns
                .Cast<DataColumn>()
                .Select(column =>
                {
                    var result = new
                    {
                        ColumnName = column.ColumnName,
                        ColumnNameNormalized = column.ColumnName.Replace(" ", string.Empty),
                    };
                    return result;
                })
                .Select(column =>
                {

                    var result = new
                    {
                        ColumnName = column.ColumnName,
                        ColumnNameNormalized = column.ColumnNameNormalized,
                        PropertyInfo = propertyInfos.FirstOrDefault(pi => pi.Name == column.ColumnNameNormalized ||
                                                                          (headersToProperties.ContainsKey(column.ColumnName.Trim()) && headersToProperties[column.ColumnName.Trim()] == pi.Name)),
                    };
                    return result;
                })
                .Where(x => x.PropertyInfo != null)
                .ToArray();

            var list = table.AsEnumerable()
                .Skip(skip)
                .AsParallel()
                .AsOrdered()
                .Select(row =>
                {
                    T obj = new T();

                    foreach (var columnMap in tableColumnMap)
                    {
                        var rowValue = row[columnMap.ColumnName];

                        var propValue = ConvertValue(rowValue, columnMap.PropertyInfo.PropertyType);
                        // If PropertyInfo object is a value type and value is null, then the property will be set to the default value for that type.
                        columnMap.PropertyInfo.SetValue(obj, propValue);
                    }

                    return obj;
                })
                .ToList();

            return list;
        }

        private static object ConvertValue(object value, Type type)
        {
            try
            {
                if (type == typeof(DateTime) || type == typeof(DateTime?))
                {
                    return ConvertToDateTime(value);
                }

                return !Convert.IsDBNull(value) && !IsNullString(value)
                    ? Convert.ChangeType(value, type)
                    : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static DateTime? ConvertToDateTime(object value)
        {
            string dateString = value.ToString();

            var cultureInfo = new CultureInfo("en-US");

            if (Convert.IsDBNull(dateString) || IsNullString(dateString))
            {
                return null;
            }

            DateTime? dateTime;
            try
            {
                dateTime = (DateTime)Convert.ChangeType(dateString, typeof(DateTime));
                return dateTime;
            }
            catch (Exception e)
            {
                try
                {
                    dateTime = DateTime.ParseExact(dateString, "D", cultureInfo);
                    return dateTime;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static bool IsNullString(object value)
        {
            return value != null && value.ToString().Equals("NULL", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <param name="headersToProperties"></param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table, Dictionary<string, string> headersToProperties) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();
                    foreach (var prop in headersToProperties)
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Value);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Key],
                                propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    list.Add(obj);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public static void CreateAndSaveExcelFile<TKey, TValue>(string filePath, Dictionary<TKey, TValue> dictionaryToBePrinted, string sheetName = "Sheet1")
        {

            List<KeyValuePair<TKey, TValue>> objects = dictionaryToBePrinted.Select(entry => entry).ToList();

            CreateAndSaveExcelFile(filePath, sheetName, objects);
        }

        public static ExcelPackage CreateAndSaveExcelFile<TRow>(string filePath, string sheetName, List<TRow> objects)
        {
            ExcelPackage excelPackage = CreateExcelFileForWrite(filePath);
            AddSheet(excelPackage, sheetName, objects);
            SaveExcelFile(excelPackage);
            return excelPackage;
        }

        public static void CreateAndSaveCsvFile<TExcel>(string filePath, string sheetName, List<TExcel> objects)
        {
            ExcelPackage excelPackage = CreateExcelFileForWrite(filePath);
            AddSheet(excelPackage, sheetName, objects);
            excelPackage.ConvertToCsv(filePath);
        }

        public static ExcelPackage CreateExcelFileForWrite(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return new ExcelPackage(new FileInfo(filePath));
        }

        public static ExcelWorksheet AddSheet<TExcel>(ExcelPackage package, string sheetName, List<TExcel> objects)
        {
            ExcelWorksheet worksheet = AddEmptySheet(package, sheetName);
            worksheet.Cells[1, 1].LoadFromCollection(objects, true, TableStyles.Medium1);
            return worksheet;
        }

        public static ExcelWorksheet ReplaceSheet<TExcel>(ExcelPackage package, string sheetName,
            List<TExcel> obejcts)
        {
            ExcelWorksheet worksheet = GetWorksheet(package, sheetName);
            if (worksheet != null)
            {
                DeleteSheet(package, sheetName);
            }

            worksheet = AddSheet(package, sheetName, obejcts);


            return worksheet;
        }

        public static void DeleteSheet(ExcelPackage package, string worksheetName)
        {
            package.Workbook.Worksheets.Delete(worksheetName);
        }

        public static ExcelWorksheet AddEmptySheet(ExcelPackage package, string sheetName)
        {
            return package.Workbook.Worksheets.Add(sheetName);
        }

        public static ExcelWorksheet GetWorksheet(ExcelPackage package, string sheetName)
        {
            return package.Workbook.Worksheets[sheetName];
        }

        public static ExcelWorksheet AddToExistsSheet<TExcel>(ExcelWorksheet worksheet, List<TExcel> obejcts, string startIndex, bool printHeaders = true, TableStyles tableStyles = TableStyles.Medium1)
        {
            worksheet.Cells[$"{startIndex}"].LoadFromCollection(obejcts, printHeaders, tableStyles);
            return worksheet;
        }

        public static ExcelWorksheet AddToExistsSheet(ExcelWorksheet worksheet, DataTable obj, string startIndex, bool printHeaders = true, TableStyles tableStyles = TableStyles.Medium1)
        {
            worksheet.Cells[$"{startIndex}"].LoadFromDataTable(obj, printHeaders, tableStyles);
            return worksheet;
        }

        public static ExcelWorksheet AddCellDataToExistsSheet<TExcel>(ExcelPackage package, string worksheetName, TExcel obj, string startIndex)
        {
            ExcelWorksheet worksheet = GetWorksheet(package, worksheetName);
            return AddCellDataToExistsSheet(worksheet, obj, startIndex);
        }

        public static ExcelWorksheet AddCellDataToExistsSheet<TExcel>(ExcelWorksheet worksheet, TExcel obj, string startIndex)
        {
            worksheet.Cells[$"{startIndex}"].Value = obj;
            return worksheet;
        }
        public static void SaveExcelFile(ExcelPackage package)
        {
            package.Save();
        }

        public static void SaveAsExcelFile(ExcelPackage package, string newName)
        {
            package.SaveAs(new FileInfo(newName));
        }

        public static byte[] SaveToBytes(ExcelPackage excelPackage)
        {
            var m = new MemoryStream();
            excelPackage.SaveAs(m);
            return m.ToArray();
        }

        public static string GetExcelColumnLatter(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;

            while (dividend > 0)
            {
                var modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        public static void MakeHeaderStyle(ExcelRange cells)
        {
            cells.Merge = true;
            cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            cells.Style.Font.Bold = true;
            cells.Style.Font.Size = cells.Style.Font.Size + 2;
            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        }

        public static void MakeCellsStyle(ExcelRange cells)
        {
            AlignCenter(cells);
        }

        public static void EmphasizeCells(ExcelRange cells)
        {
            cells.Style.Font.Bold = true;
            cells.Style.Font.Color.SetColor(Color.DarkRed);
        }

        public static void AlignCenter(ExcelRange cells)
        {
            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        public static int GetLastFilledRow(ExcelWorksheet workSheet)
        {
            return workSheet.Cells.Last(cell => !cell.Value.ToString().Equals("")).End.Row;
        }

        public static void ConvertToCsv(this ExcelPackage package, string targetFile)
        {
            var worksheet = package.Workbook.Worksheets[1];

            var maxColumnNumber = worksheet.Dimension.End.Column;
            var currentRow = new List<string>(maxColumnNumber);
            var totalRowCount = worksheet.Dimension.End.Row;
            var currentRowNum = 1;

            using (var writer = new StreamWriter(targetFile, false, Encoding.UTF8))
            {
                while (currentRowNum <= totalRowCount)
                {
                    BuildRow(worksheet, currentRow, currentRowNum, maxColumnNumber);
                    WriteRecordToFile(currentRow, writer, currentRowNum, totalRowCount);
                    currentRow.Clear();
                    currentRowNum++;
                }
            }
        }

        private static void WriteRecordToFile(List<string> record, StreamWriter sw, int rowNumber, int totalRowCount)
        {
            var commaDelimitedRecord = record.ToDelimitedString(",");

            if (rowNumber == totalRowCount)
            {
                sw.Write(commaDelimitedRecord);
            }
            else
            {
                sw.WriteLine(commaDelimitedRecord);
            }
        }

        private static void BuildRow(ExcelWorksheet worksheet, List<string> currentRow, int currentRowNum, int maxColumnNumber)
        {
            for (int i = 1; i <= maxColumnNumber; i++)
            {
                var cell = worksheet.Cells[currentRowNum, i];
                if (cell == null)
                {
                    // add a cell value for empty cells to keep data aligned.
                    AddCellValue(string.Empty, currentRow);
                }
                else
                {
                    AddCellValue(GetCellText(cell), currentRow);
                }
            }
        }

        private static string DuplicateTicksForSql(this string s)
        {
            return s.Replace("'", "''");
        }

        public static string ToDelimitedString(this List<string> list, string delimiter = ":", bool insertSpaces = false, string qualifier = "", bool duplicateTicksForSQL = false)
        {
            var result = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string initialStr = duplicateTicksForSQL ? list[i].DuplicateTicksForSql() : list[i];
                result.Append((qualifier == string.Empty) ? initialStr : string.Format("{1}{0}{1}", initialStr, qualifier));
                if (i < list.Count - 1)
                {
                    result.Append(delimiter);
                    if (insertSpaces)
                    {
                        result.Append(' ');
                    }
                }
            }
            return result.ToString();
        }

        private static string GetCellText(ExcelRangeBase cell)
        {
            return cell.Value == null ? string.Empty : cell.Value.ToString();
        }

        private static void AddCellValue(string s, List<string> record)
        {
            record.Add(string.Format("{0}{1}{0}", '"', s));
        }

        private static Dictionary<string, string> GetHeadersToPropertiesMapping(Type type)
        {
            Dictionary<string, string> headersToProperties = new Dictionary<string, string>();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                Attribute customAttribute = property.GetCustomAttribute(typeof(ExcelColumnHeaderAttribute), true);
                if (customAttribute is ExcelColumnHeaderAttribute attribute)
                {
                    ExcelColumnHeaderAttribute excelColumnHeader = attribute;
                    headersToProperties.Add(excelColumnHeader.Header, property.Name);
                }
            }

            return headersToProperties;
        }

        private static bool IsAllNullOrEmpty(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return true;

            return obj.GetType().GetProperties()
                .All(x => IsNullOrEmpty(x.GetValue(obj)));
        }

        private static bool IsNullOrEmpty(object value)
        {
            if (object.ReferenceEquals(value, null))
                return true;

            var type = value.GetType();
            return type.IsValueType
                   && object.Equals(value, Activator.CreateInstance(type));
        }
    }
}
