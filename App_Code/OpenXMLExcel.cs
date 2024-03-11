using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

using System.Web.UI;
using ZedService;
using System.Data.SqlClient;
using System.Data.OleDb;

using System.Web;
using OfficeOpenXml;
using System.Xml;
using ExportExcelOpenXML;

//using OfficeOpenXml;
//using System.Xml;
//using ExportExcelOpenXML;


/* Library: OpenXML Excel Creation Library
 * Requirements: A blank xslx template with maximum sheets you may need
 * Parameters required: Dataset containing datatables to export, exportfilelocation(without file name),
 *                      location of template(with file name),name of the file to be exported,
 *                      bool value that signifies whether Header needs to be included. 
 *                      In case of false first row will be skipped and custom headers can be specified 
 *                      in template file.
 *Created by: Aman Nigam                      
 *Date Completed:8/17/2010
 *
 * 14-Jun-2011, Prashant chitransh, #Ch01:  Change in excell to dataset converssion function, because record were moving to blank columns.
 * 30-Aug-2011, Rakesh Goel, #CC02 : Datetime column type handling added (code shared by Amit Srivastav)
 * ------------------------ADD----------------------
 * 07-Aug-2012, Arpit Bhatnagar
 * New Fuction add:-  For CSV File To return dataset
 * 25-May-2015, Sumit Maurya,#CC03 ,Code Commented and added with return type false in Response.Redirect 
 * 14-Sep-2015, Balram Jha,#CC04 - Export to Excel modification to avoid error message of invalid file.
 * 25-Feb-2016, Balram Jha,#CC05 - Save CSV file method added
 * 21-Feb-2017, Vijay Katiyar, #CC06 - Added ImportExcelFileV2() method overload pass filename & RelativePath to open file 
 * 17-Jan-2018, Sumit Maurya, #CC07 - New overload craeted existing one was not working in sales project.
 * 28-Jun-2018, Sumit Maurya, #CC08, DateTime Format made configurable instead of hardcode.
 
 */
namespace ZedService
{
    public struct My
    {
        static My()
        {
            throw new NotImplementedException();
        }
    }
    public class OpenXMLExcel
    {
        uint doubleStyleId, intStyleId, datetimeStyleId, headerId, longStyleId;
        string relID;
        private static List<char> Letters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ' };


        public static int? GetColumnIndexFromName(string columnName)        // #Ch01: added
        {
            int? columnIndex = null;

            string[] colLetters = Regex.Split(columnName, "([A-Z]+)");
            colLetters = colLetters.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (colLetters.Count() <= 2)
            {
                int index = 0;
                foreach (string col in colLetters)
                {
                    List<char> col1 = colLetters.ElementAt(index).ToCharArray().ToList();
                    int? indexValue = Letters.IndexOf(col1.ElementAt(index));

                    if (indexValue != -1)
                    {
                        // The first letter of a two digit column needs some extra calculations
                        if (index == 0 && colLetters.Count() == 2)
                        {
                            columnIndex = columnIndex == null ? (indexValue + 1) * 26 : columnIndex + ((indexValue + 1) * 26);
                        }
                        else
                        {
                            columnIndex = columnIndex == null ? indexValue : columnIndex + indexValue;
                        }
                    }
                    index++;
                }
            }

            return columnIndex;
        }

        public static string GetColumnName(string cellReference)        // #Ch01: added
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        public string uploadFile(System.Web.HttpPostedFile txtFile)
        {
            try
            {
                string strFileName = null;
                string strFileNamePath = null;
                string strFileFolder = null;
                string strExtension = null;

                strFileFolder = System.Web.HttpContext.Current.Server.MapPath("~") + "/uploaddownload/UploadTemp/";

                strFileName = System.IO.Path.GetFileName(txtFile.FileName);

                strExtension = System.IO.Path.GetExtension(strFileName);
                if (strExtension == ".xlsx")
                {
                    DateTime myDate = DateTime.Now;
                    string myTime = ((((((DateTime.Now.Year.ToString() + "_") + DateTime.Now.Month.ToString() + "_") + DateTime.Now.Day.ToString() + "_") + DateTime.Now.Hour.ToString() + "_") + DateTime.Now.Minute.ToString() + "_") + DateTime.Now.Second.ToString() + "_") + System.DateTime.Now.Ticks.ToString();

                    strFileNamePath = strFileFolder + myTime + strFileName.Replace(" ", "");
                    txtFile.SaveAs(strFileNamePath);

                    return myTime + strFileName.Replace(" ", "");
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                //  clsException.clsHandleException.fncHandleException(ex, ("User Id- '" + Session["webuserid"] + "' User - '") + Session["person_name"] + "'");
                return "";
            }
        }
        
        /// <summary>
        /// Imprort Code on upload excel file
        /// </summary>
        /// <param name="txtFile"></param>
        /// <returns></returns>
        public DataSet ImportExcelFile(System.Web.HttpPostedFile txtFile)         // #Ch01: added
        {
            DataSet ds = new DataSet();
            string filename = "";
            string fname = "";
            fname = uploadFile(txtFile);
            filename = System.Web.HttpContext.Current.Server.MapPath("~") + "/uploaddownload/UploadTemp/" + fname;

            DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadSheet = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(filename, true);
            using (spreadSheet)
            {
                WorkbookPart workbook = spreadSheet.WorkbookPart;
                IEnumerable<Sheet> sheets = workbook.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

                for (int i = 0; i < sheets.Count(); i++)
                {
                    ds.Tables.Add();
                    string rId = sheets.ElementAt(i).Id;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadSheet.WorkbookPart.GetPartById(rId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();

                    if (rows.Count() > 1)
                    {
                        foreach (Cell cell in rows.ElementAt(0))
                        {
                            ds.Tables[i].Columns.Add(GetCellValue(spreadSheet, cell));
                        }
                        int check = 0;

                        for (int k = 1; k <= rows.Count(); k++)
                        {
                            int cellCount = 0;
                            {
                                if (check != 0)
                                {
                                    DataRow dr = ds.Tables[i].NewRow();

                                    foreach (Cell cell in rows.ElementAt(k - 1).Descendants<Cell>())
                                    {
                                        int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));

                                        if (cellCount < cellColumnIndex)
                                        {
                                            do
                                            {
                                                dr[cellCount] = null;
                                                cellCount++;
                                            }
                                            while (cellCount < cellColumnIndex);
                                        }
                                        dr[cellCount] = GetCellValue(spreadSheet, cell);
                                        cellCount++;
                                    }
                                    ds.Tables[i].Rows.Add(dr);
                                }
                                check++;
                            }
                        }
                    }
                }
            }
            return ds;
        }
        public DataSet ImportExcelFileV2(System.Web.HttpPostedFile txtFile)
        {
            string filelocation = "";
            string filename = "";
            string fname = "";
            fname = uploadFile(txtFile);
            filename = System.Web.HttpContext.Current.Server.MapPath("~") + "/uploaddownload/UploadTemp/" + fname;
            filelocation = filename;

            string excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filelocation + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"";
            OleDbConnection dbCon = new OleDbConnection(excelConnectionString);
            dbCon.Open();
            DataTable dtSheetName = dbCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            DataSet dsOutput = new DataSet();
            for (int nCount = 0; nCount < dtSheetName.Rows.Count; nCount++)
            {
                string sSheetName = dtSheetName.Rows[nCount]["TABLE_NAME"].ToString();
                if (sSheetName.ToLower() != "help$")
                {
                    string sQuery = "Select * From [" + sSheetName + "]";
                    OleDbCommand dbCmd = new OleDbCommand(sQuery, dbCon);
                    OleDbDataAdapter dbDa = new OleDbDataAdapter(dbCmd);
                    DataTable dtData = new DataTable();
                    dbDa.Fill(dtData);
                    dsOutput.Tables.Add(dtData);
                }

            }
            dbCon.Close();
            DataSet ds = new DataSet();
            foreach (DataTable dtCheck in dsOutput.Tables)
            {
                if (dtCheck.Rows.Count != 0)
                {
                    ds.Merge(dtCheck);
                }
            }
            return ds;
        }
        public DataSet ImportExcelFileV3(string fname)
        {
            DataSet ds = new DataSet();
            string filename = "";
            filename = System.Web.HttpContext.Current.Server.MapPath("~") + "/Excel/Upload/UploadExcelFiles/" + fname;

            DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadSheet = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(filename, true);
            using (spreadSheet)
            {
                WorkbookPart workbook = spreadSheet.WorkbookPart;
                IEnumerable<Sheet> sheets = workbook.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

                for (int i = 0; i < sheets.Count(); i++)
                {
                    ds.Tables.Add();
                    string rId = sheets.ElementAt(i).Id;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadSheet.WorkbookPart.GetPartById(rId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();

                    if (rows.Count() > 1)
                    {
                        foreach (Cell cell in rows.ElementAt(0))
                        {
                            ds.Tables[i].Columns.Add(GetCellValue(spreadSheet, cell));
                        }
                        int check = 0;

                        for (int k = 1; k <= rows.Count(); k++)
                        {
                            int cellCount = 0;
                            {
                                if (check != 0)
                                {
                                    DataRow dr = ds.Tables[i].NewRow();

                                    foreach (Cell cell in rows.ElementAt(k - 1).Descendants<Cell>())
                                    {
                                        int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));

                                        if (cellCount < cellColumnIndex)
                                        {
                                            do
                                            {
                                                dr[cellCount] = null;
                                                cellCount++;
                                            }
                                            while (cellCount < cellColumnIndex);
                                        }
                                        dr[cellCount] = GetCellValue(spreadSheet, cell);
                                        cellCount++;
                                    }
                                    ds.Tables[i].Rows.Add(dr);
                                }
                                check++;
                            }
                        }
                    }
                }
            }
            return ds;
        }

        /*#CC06 Added start*/

        public DataSet ImportExcelFileV2(string filePathWithName)
        {
            try
            {
                string filelocation = filePathWithName;

                string excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filelocation + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"";
                OleDbConnection dbCon = new OleDbConnection(excelConnectionString);
                dbCon.Open();
                DataTable dtSheetName = dbCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                DataSet dsOutput = new DataSet();
                for (int nCount = 0; nCount < dtSheetName.Rows.Count; nCount++)
                {
                    string sSheetName = dtSheetName.Rows[nCount]["TABLE_NAME"].ToString();
                    if (sSheetName.ToLower() != "help$")
                    {
                        string sQuery = "Select * From [" + sSheetName + "]";
                        OleDbCommand dbCmd = new OleDbCommand(sQuery, dbCon);
                        OleDbDataAdapter dbDa = new OleDbDataAdapter(dbCmd);
                        DataTable dtData = new DataTable();
                        dbDa.Fill(dtData);
                        dsOutput.Tables.Add(dtData);
                    }

                }
                dbCon.Close();
                DataSet ds = new DataSet();
                foreach (DataTable dtCheck in dsOutput.Tables)
                {
                    if (dtCheck.Rows.Count != 0)
                    {
                        ds.Merge(dtCheck);
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*#CC06 Added End*/

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue != null)
            {
                string value = cell.CellValue.InnerXml;

                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Export to Excel Code
        /// </summary>
        /// <param name="myDS"></param>
        /// <param name="exportFileLocation"></param>
        /// <param name="templateLocation"></param>
        /// <param name="exportFileName"></param>
        /// <param name="includeHeader"></param>
        /// <param name="totalSheets"></param>
        public void ExportDataTable(DataSet myDS, string exportFileLocation, string templateLocation, string exportFileName, bool includeHeader, int totalSheets)
        {
            string destfile = exportFileLocation + exportFileName;
            File.Copy(templateLocation, exportFileLocation + exportFileName, true);
            SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(exportFileLocation + exportFileName, true);
            using (spreadSheet)//SpreadsheetDocument spreadSheet =
            //SpreadsheetDocument.Open(exportFileLocation + "/" + exportFileName, true))
            {
                Workbook wb = new Workbook();
                WorkbookPart workbook = spreadSheet.WorkbookPart;
                Stylesheet style = workbook.WorkbookStylesPart.Stylesheet;
                Sheets sheets = new Sheets();
                int DsCount = 0;
                for (int s = totalSheets - 1; s >= 0; s--)
                {
                    if (DsCount + 1 > myDS.Tables.Count)
                        break;
                    WorksheetPart sheet = workbook.WorksheetParts.ElementAt(s);
                    SheetData data = sheet.Worksheet.GetFirstChild<SheetData>();
                    Row headerRow = new Row();
                    headerRow.RowIndex = 1;

                    relID = workbook.GetIdOfPart(sheet);
                    doubleStyleId = createCellFormat(style, null, null, UInt32Value.FromUInt32(4));
                    intStyleId = createCellFormat(style, null, null, UInt32Value.FromUInt32(3));
                    // longStyleId = createCellFormat(style, null, null, UInt32Value.FromUInt32(3));
                    datetimeStyleId = createCellFormat(style, null, null, UInt32Value.FromUInt32(14));  //#CC02 modified from 24 to 14

                    headerId = createCellStyle(style, "Arial", System.Drawing.Color.White, System.Drawing.Color.DarkGray);

                    if (includeHeader)
                    {
                        for (int i = 0; i < myDS.Tables[DsCount].Columns.Count; i++)
                        {
                            Cell headerCell = createTextCell(i + 1, 1, myDS.Tables[DsCount].Columns[i].ColumnName, headerId);

                            headerRow.AppendChild(headerCell);
                        }
                        data.AppendChild(headerRow);
                    }
                    DataRow contentRow;
                    for (int i = 0; i < myDS.Tables[DsCount].Rows.Count; i++)
                    {
                        contentRow = myDS.Tables[DsCount].Rows[i];
                        data.AppendChild(createContentRow(contentRow, i + 2));
                    }
                    Sheet newsheet = new Sheet { Name = myDS.Tables[DsCount].TableName.ToString(), SheetId = Convert.ToUInt32(s + 1), Id = relID };
                    sheets.Append(newsheet);
                    DsCount++;
                }

                wb.Append(sheets);
                spreadSheet.WorkbookPart.Workbook = wb;
                spreadSheet.WorkbookPart.Workbook.Save();
                spreadSheet.Close();
            }

        }
        /* #CC07 Add Start */
       public void ExportDataTableV2(DataSet myDS, string exportFileLocation, string templateLocation, string exportFileName, bool includeHeader, int totalSheets)
        {
            
            using (ExcelPackage pck = new ExcelPackage())
            {
                
                String strSheetName;//#CC01
                for (int i = 0; i < myDS.Tables.Count; i++)
                {
                    // strSheetName = "Sheet" + Convert.ToString(i);//old code
                    strSheetName = myDS.Tables[i].TableName;//wanted to show the name given by us(user friendly name)
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(strSheetName);
                   

                    var timeColumns = from DataColumn d in myDS.Tables[i].Columns
                                      where d.DataType == typeof(DateTime)
                                      select d.Ordinal + 1;

                    foreach (int column in timeColumns)
                    {
                        ws.Cells[2, column, myDS.Tables[i].Rows.Count + 1, column].Style.Numberformat.Format = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ExportToExcelGlobalDatetimeFormat"])/* #CC08 Added */; /* "m/d/yyyy h:mm AM/PM"; #CC08 Commented*/;
                    }

                    var time = from DataColumn d in myDS.Tables[i].Columns
                               where d.DataType == typeof(TimeSpan)
                               select d.Ordinal + 1;

                    

                    foreach (int column1 in time)
                    {
                        ws.Cells[2, column1, myDS.Tables[i].Rows.Count + 1, column1].Style.Numberformat.Format = (System.Configuration.ConfigurationManager.AppSettings["ExportToExcelGlobalTimeFormat"])/* #CC08 Added */; /*"hh:mm AM/PM"; #CC08 Commented */
                        
                    }
                    //Header rows don't affect autosize.
                    // ws.Cells[2, 1, myDS.Tables[i].Rows.Count + 2, myDS.Tables[i].Columns.Count + 1].AutoFitColumns(); //(5, 50);
                    ws.Cells["A1"].LoadFromDataTable(myDS.Tables[i], true, OfficeOpenXml.Table.TableStyles.Medium15);


                }
                byte[] fileBytes = pck.GetAsByteArray();
                //Write it back to the client
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Length", fileBytes.Length.ToString());
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + exportFileName);
                //HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray());
                HttpContext.Current.Response.BinaryWrite(fileBytes);
                //HttpContext.Current.Response.End();
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                //HttpContext.Current.Response.Close();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        /* #CC07 Add End */

        public void ExportDataTableV2(DataSet myDS, string exportFileLocation, string templateLocation, string exportFileName, bool includeHeader, int totalSheets, string[] SheetName)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                String strSheetName;//#CC01
                for (int i = 0; i < myDS.Tables.Count; i++)
                {
                    //Create the worksheet
                    ////#CC01 start
                    if (totalSheets == 1 && SheetName == null)
                    {
                        strSheetName = "Sheet" + Convert.ToString(i);
                    }
                    else
                    {
                        strSheetName = SheetName[i].ToString();
                    }
                    ////#CC01 end
                    //String strSheetName = "Sheet" + Convert.ToString(i);
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(strSheetName);
                    #region Not In Use
                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1

                    /*
                     * &&
                                    (d.ColumnName.EndsWith("Time") || d.ColumnName.EndsWith("UTC")
                     */

                    /*var _strStringColumns = from DataColumn d in myDS.Tables[i].Columns
                                            where d.DataType == typeof(String)
                                            select d.Ordinal + 1;
                     foreach (int column in _strStringColumns)
                    {
                        ws.Column(column).Style.Numberformat.Format = "";
                    }*/
                    #endregion
                    var timeColumns = from DataColumn d in myDS.Tables[i].Columns
                                      where d.DataType == typeof(DateTime)
                                      select d.Ordinal + 1;

                    foreach (int column in timeColumns)
                    {
                        ws.Cells[2, column, myDS.Tables[i].Rows.Count + 1, column].Style.Numberformat.Format = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ExportToExcelGlobalDatetimeFormat"])/* #CC08 Added */; /* "m/d/yyyy h:mm AM/PM"; #CC08 Commented*/
                    }
                    var time = from DataColumn d in myDS.Tables[i].Columns
                               where d.DataType == typeof(TimeSpan)
                               select d.Ordinal + 1;

                    foreach (int column1 in time)
                    {
                        ws.Cells[2, column1, myDS.Tables[i].Rows.Count + 1, column1].Style.Numberformat.Format = (System.Configuration.ConfigurationManager.AppSettings["ExportToExcelGlobalTimeFormat"])/* #CC08 Added */; /*"hh:mm AM/PM"; #CC08 Commented */

                    }
                    //Header rows don't affect autosize.
                    // ws.Cells[2, 1, myDS.Tables[i].Rows.Count + 2, myDS.Tables[i].Columns.Count + 1].AutoFitColumns(); //(5, 50);
                    ws.Cells["A1"].LoadFromDataTable(myDS.Tables[i], true, OfficeOpenXml.Table.TableStyles.Medium15);


                }
                //Write it back to the client
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + exportFileName);
                HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray());
                //HttpContext.Current.Response.End();
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;//#CC04 added
                //HttpContext.Current.Response.Close();//#CC04 comented
                HttpContext.Current.ApplicationInstance.CompleteRequest();//#CC04 added
            }
        }
        private uint createCellStyle(Stylesheet ss, string fontName, System.Drawing.Color fontColor, System.Drawing.Color fillColor)
        {
            uint fontID = createFont(ss, "Arial", null, false, System.Drawing.Color.White);
            uint fillID = createFill(ss, System.Drawing.Color.DarkGray);
            uint headerId = createCellFormat(ss, fontID, fillID, null);
            return headerId;
        }
        private Cell createTextCell(int columnIndex, int rowIndex, object cellValue, Nullable<uint> StyleIndex)
        {
            Cell cell = new Cell();
            cell.DataType = CellValues.InlineString;
            cell.CellReference = getColumnName(columnIndex) + rowIndex;
            if (StyleIndex.HasValue)
                cell.StyleIndex = StyleIndex;
            InlineString inlineString = new InlineString();
            Text t = new Text();
            t.Text = cellValue.ToString();
            inlineString.AppendChild(t);
            cell.AppendChild(inlineString);
            return cell;
        }
        private Row createContentRow(DataRow dataRow, int rowIndex)
        {
            Row row = new Row

            {
                RowIndex = (UInt32)rowIndex
            };

            int intVal;
            DateTime datetimeVal;

            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                Cell dataCell = null;
                String dataType = dataRow.Table.Columns[i].DataType.ToString();
                switch (dataType)
                {

                    //case "System.DateTime": 
                    //    if (DateTime.TryParse(dataRow[i].ToString(), out datetimeVal))
                    //    {
                    //        dataCell = createValueCell(i + 1, rowIndex, dataRow[i], datetimeStyleId);
                    //    }
                    //    break;


                    case "System.DateTime":             //#CC02 added this case block
                        if (DateTime.TryParse(dataRow[i].ToString(), out datetimeVal))
                        {
                            dataCell = createValueCell(i + 1, rowIndex, datetimeVal.ToOADate().ToString(), datetimeStyleId);
                        }
                        break;


                    case "System.Int16":
                        if (int.TryParse(dataRow[i].ToString(), out intVal))
                        {
                            dataCell = createValueCell(i + 1, rowIndex, dataRow[i], intStyleId);
                        }
                        break;
                    case "System.Int32":
                        if (int.TryParse(dataRow[i].ToString(), out intVal))
                        {
                            dataCell = createValueCell(i + 1, rowIndex, dataRow[i], intStyleId);
                        }
                        break;
                    case "System.Int64":
                        if (int.TryParse(dataRow[i].ToString(), out intVal))
                        {
                            dataCell = createValueCell(i + 1, rowIndex, dataRow[i], intStyleId);
                        }
                        break;
                    case "System.Long":
                        if (int.TryParse(dataRow[i].ToString(), out intVal))
                        {
                            dataCell = createValueCell(i + 1, rowIndex, dataRow[i], intStyleId);
                        }
                        break;
                    case "System.Double":
                        if (int.TryParse(dataRow[i].ToString(), out intVal))
                        {
                            dataCell = createValueCell(i + 1, rowIndex, dataRow[i], intStyleId);
                        }
                        break;
                    case "System.String":
                        dataCell = createTextCell(i + 1, rowIndex, dataRow[i], null);
                        break;
                    default:
                        dataCell = createTextCell(i + 1, rowIndex, dataRow[i], null);
                        break;
                }

                row.AppendChild(dataCell);
            }
            return row;
        }
        private UInt32 createFont(Stylesheet style, string fontName, Nullable<double> fontSize, bool isBold, System.Drawing.Color color)
        {
            Font fontCreated = new Font();
            if (fontName != null)
            {
                FontName name = new FontName();
                name.Val = fontName;
                fontCreated.Append(name);
            }
            if (fontSize != null)
            {
                FontSize size = new FontSize();
                size.Val = fontSize;
                fontCreated.Append(size);
            }
            if (isBold)
            {
                Bold bold = new Bold();
                fontCreated.Append(bold);
            }
            if (color != null)
            {
                Color myColor = new Color()
                {
                    Rgb = new HexBinaryValue()
                    {
                        Value =
                            System.Drawing.ColorTranslator.ToHtml(
                                System.Drawing.Color.FromArgb(
                                    color.A,
                                    color.R,
                                    color.G,
                                    color.B)).Replace("#", "")
                    }
                };
                fontCreated.Append(myColor);
            }
            style.Fonts.Append(fontCreated);
            UInt32 res = style.Fonts.Count;
            style.Fonts.Count++;
            return res;


        }
        private string getColumnName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = String.Empty;
            int modifier;

            while (dividend > 0)
            {
                modifier = (dividend - 1) % 26;
                columnName =
                    Convert.ToChar(65 + modifier).ToString() + columnName;
                dividend = (int)((dividend - modifier) / 26);
            }

            return columnName;
        }
        private Cell createValueCell(int columnIndex, int rowIndex, object cellValue, Nullable<uint> styleIndex)
        {
            Cell cell = new Cell();
            cell.CellReference = getColumnName(columnIndex) + rowIndex;
            CellValue value = new CellValue();
            value.Text = cellValue.ToString();

            //apply the cell style if supplied
            if (styleIndex.HasValue)
                cell.StyleIndex = styleIndex.Value;

            cell.AppendChild(value);

            return cell;
        }
        private UInt32Value createFill(Stylesheet styleSheet, System.Drawing.Color fillColor)
        {
            Fill fill = new Fill(
                new PatternFill(
                   new ForegroundColor()
                   {
                       Rgb = new HexBinaryValue()
                       {
                           Value =
                           System.Drawing.ColorTranslator.ToHtml(
                               System.Drawing.Color.FromArgb(
                                  fillColor.A,
                                   fillColor.R,
                                    fillColor.G,
                                     fillColor.B)).Replace("#", "")
                       }
                   })
                {
                    PatternType = PatternValues.Solid
                }
           );
            styleSheet.Fills.Append(fill);

            UInt32Value result = styleSheet.Fills.Count;
            styleSheet.Fills.Count++;
            return result;
        }
        private UInt32Value createCellFormat(Stylesheet styleSheet, UInt32Value fontIndex, UInt32Value fillIndex, UInt32Value numberFormatId)
        {
            CellFormat cellFormat = new CellFormat();

            if (fontIndex != null)
                cellFormat.FontId = fontIndex;

            if (fillIndex != null)
                cellFormat.FillId = fillIndex;

            if (numberFormatId != null)
            {
                cellFormat.NumberFormatId = numberFormatId;
                cellFormat.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            }

            styleSheet.CellFormats.Append(cellFormat);

            UInt32Value result = styleSheet.CellFormats.Count;
            styleSheet.CellFormats.Count++;
            return result;
        }
        public DataSet ImportExcelFileSerialNumber(System.Web.HttpPostedFile txtFile)         // #Ch01: added
        {
            DataSet ds = new DataSet();

            //DataTable dt = new DataTable();
            //dt.Columns.Add("CountryID", typeof(int));
            //dt.Columns.Add("ModelId", typeof(int));
            //dt.Columns.Add("ShipmentDate", typeof(DateTime));
            //dt.Columns.Add("SerialNumber", typeof(string));
            //dt.Columns.Add("SerialCameInShipment", typeof(int));

            string filename = "";
            string fname = "";
            fname = uploadFile(txtFile);
            filename = System.Web.HttpContext.Current.Server.MapPath("~") + "/uploaddownload/UploadTemp/" + fname;

            DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadSheet = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(filename, true);
            using (spreadSheet)
            {
                WorkbookPart workbook = spreadSheet.WorkbookPart;
                IEnumerable<Sheet> sheets = workbook.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

                for (int i = 0; i < sheets.Count(); i++)
                {
                    ds.Tables.Add();
                    string rId = sheets.ElementAt(i).Id;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadSheet.WorkbookPart.GetPartById(rId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();

                    if (rows.Count() > 1)
                    {
                        foreach (Cell cell in rows.ElementAt(0))
                        {
                            ds.Tables[i].Columns.Add(GetCellValue(spreadSheet, cell));
                        }
                        int check = 0;

                        for (int k = 1; k <= rows.Count(); k++)
                        {
                            int cellCount = 0;
                            {
                                if (check != 0)
                                {
                                    DataRow dr = ds.Tables[i].NewRow();

                                    foreach (Cell cell in rows.ElementAt(k - 1).Descendants<Cell>())
                                    {
                                        int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));


                                        if (cellCount < cellColumnIndex)
                                        {
                                            do
                                            {
                                                dr[cellCount] = null;
                                                cellCount++;
                                            }
                                            while (cellCount < cellColumnIndex);
                                        }
                                        if (cellCount == 2)
                                        {
                                            cell.DataType = CellValues.Date;
                                            cell.CellValue = new CellValue(DateTime.Now.ToString());
                                            cell.StyleIndex = 1;
                                            dr[cellCount] = GetCellValue(spreadSheet, cell);

                                        }
                                        else
                                        {
                                            dr[cellCount] = GetCellValue(spreadSheet, cell);
                                        }
                                        cellCount++;
                                    }
                                    ds.Tables[i].Rows.Add(dr);
                                }
                                check++;
                            }
                        }
                    }
                }
            }
            return ds;
        }
        public DataSet ImportCSVFile(string FilePath, bool IsHeader, int NumberColumn)
        {
            DataSet ds = new DataSet();
            try
            {
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    DataTable dt = new DataTable();
                    DataTable dtmain = new DataTable();
                    FileInfo f = new FileInfo(FilePath);

                    if (f.Extension != ".csv" && f.Extension != ".CSV")
                    {
                        ds = null;
                        return ds;
                    }
                    if (IsHeader == true)
                    {
                        string line = sr.ReadLine();
                        line = line.Replace("\"", "");
                        string[] value = line.Split(',');
                        DataRow row;
                        foreach (string dc in value)
                        {
                            dt.Columns.Add(new DataColumn(dc));
                        }
                        while (!sr.EndOfStream)
                        {
                            value = sr.ReadLine().Replace("\"", "").Split(',');
                            if (value.Length == NumberColumn)
                            {
                                row = dt.NewRow();
                                row.ItemArray = value;
                                dt.Rows.Add(row);
                            }
                            else
                            {
                                if (value.Length != 1)
                                {
                                    throw new System.ArgumentException("In Correct File Format");
                                }
                            }

                        }
                    }
                    else
                    {
                        string line = sr.ReadLine();
                        if (line == "")
                        {
                            throw new System.ArgumentException("First Row of file can't be blank!!!");
                        }
                        string[] value = line.Split(',');
                        if (value.Length != NumberColumn)
                        {
                            throw new System.ArgumentException("In Correct File Format");
                        }

                        for (int i = 0; i < value.Length; i++)
                        {
                            dt.Columns.Add("Column" + i);
                        }
                        using (StreamReader sr1 = new StreamReader(FilePath))
                        {
                            string[] value1;
                            DataRow row;
                            while (!sr1.EndOfStream)
                            {
                                value1 = sr1.ReadLine().Replace("\"", "").Split(',');
                                if (value1.Length == NumberColumn)
                                {
                                    row = dt.NewRow();
                                    row.ItemArray = value1;
                                    dt.Rows.Add(row);
                                }
                                else
                                {
                                    if (value1.Length != 1)
                                    {
                                        throw new System.ArgumentException("In Correct File Format for SAP_RPSI_ID: " + value1[0].ToString());
                                    }
                                }
                            }
                        }
                    }
                    ds.Merge(dt);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                ds = null;
                throw ex;
                return ds;
            }
        }
        public void ExportCSV(DataTable data, string fileName, bool isheader)
        {

            //try
            //{

            StringBuilder str = new StringBuilder();
            if (isheader == true)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    str.Append(data.Columns[i].ColumnName.ToString() + ",");
                }
                str.Replace(",", Environment.NewLine, str.Length - 1, 1);
            }
            foreach (DataRow dr in data.Rows)
            {
                foreach (object field in dr.ItemArray)
                {
                    str.Append(field.ToString() + ",");
                }
                str.Replace(",", Environment.NewLine, str.Length - 1, 1);
            }
            string strVPath = "~/UploadDownload/UploadPersistent/SAPFOLDER/" + fileName + System.DateTime.Now.Ticks.ToString() + ".csv";//
            string strPath = HttpContext.Current.Server.MapPath(strVPath);
            File.WriteAllText(strPath, str.ToString());
            HttpContext.Current.Response.Redirect(strVPath);
            #region Not in use old code
            /*
            HttpContext context = HttpContext.Current;

            context.Response.Clear();

            context.Response.ContentType = "text/csv";

            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".csv");

            //rite column header names  
            if (isheader == true) // file with header
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {

                    if (i > 0)
                    {

                        context.Response.Write(",");

                    }

                    context.Response.Write(data.Columns[i].ColumnName);

                }

                context.Response.Write(Environment.NewLine);

                //Write data  

                foreach (DataRow row in data.Rows)
                {

                    for (int i = 0; i < data.Columns.Count; i++)
                    {

                        if (i > 0)
                        {

                            context.Response.Write(",");

                        }

                        context.Response.Write(row[i].ToString());

                    }

                    context.Response.Write(Environment.NewLine);
                }
                context.Response.End();
            }
            else
            {
                foreach (DataRow row in data.Rows)
                {

                    for (int i = 0; i < data.Columns.Count; i++)
                    {

                        if (i > 0)
                        {

                            context.Response.Write(",");

                        }

                        context.Response.Write(row[i].ToString());

                    }

                    context.Response.Write(Environment.NewLine);

                }

                ////string strPath = context.Server.MapPath("~/UploadDownload/UploadPersistent/SAPFOLDER/" + fileName + System.DateTime.Now.Ticks.ToString() + ".csv");
                //context.Response.TransmitFile(strPath);
                ////context.Request.SaveAs(strPath, true);
                //context.Response.WriteFile(strPath);
                //context.RewritePath(strPath);
                context.Response.End();
            }
           

            // }

            //catch (Exception ex)
            //{

            //}*/
            #endregion
        }
        public void ExportCSVTextDelimited(DataTable data, string fileName, bool isheader)
        {

            //try
            //{

            StringBuilder str = new StringBuilder();
            if (isheader == true)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    str.Append(data.Columns[i].ColumnName.ToString() + ",");
                }
                str.Replace(",", Environment.NewLine, str.Length - 1, 1);
            }
            foreach (DataRow dr in data.Rows)
            {
                foreach (object field in dr.ItemArray)
                {
                    string celltext = field.ToString().Replace("\"", "\"\"");  //replace " with ""
                    if (celltext.IndexOf(",", 0) != -1)  //if comma in text, delimit it between "
                    {
                        celltext = "\"" + celltext + "\"";
                    }

                    str.Append(celltext + ",");
                }
                str.Replace(",", Environment.NewLine, str.Length - 1, 1);
            }
            string strVPath = "~/UploadDownload/UploadPersistent/SAPFOLDER/" + fileName + System.DateTime.Now.Ticks.ToString() + ".csv";//
            string strPath = HttpContext.Current.Server.MapPath(strVPath);
            File.WriteAllText(strPath, str.ToString());
            HttpContext.Current.Response.Redirect(strVPath);

        }
        public void ExportCSVFilePath(DataTable data, string strVPath, bool isheader)
        {
            StringBuilder str = new StringBuilder();
            Int32 Howmanyrows = 0;
            Int32 counter = 0;
            Howmanyrows = data.Rows.Count;
            if (isheader == true)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    str.Append(data.Columns[i].ColumnName.ToString() + ",");
                }
                str.Replace(",", Environment.NewLine, str.Length - 1, 1);
            }
            foreach (DataRow dr in data.Rows)
            {
                counter = counter + 1;
                foreach (object field in dr.ItemArray)
                {
                    str.Append(field.ToString() + ",");
                }
                if (Howmanyrows == counter)/*Now it is last row of the data*/
                    str.Replace(",", "", str.Length - 1, 1);
                else
                    str.Replace(",", Environment.NewLine, str.Length - 1, 1);
            }

            //string strVPath = "~/UploadDownload/UploadPersistent/SAPFOLDER/" + fileName + System.DateTime.Now.Ticks.ToString() + ".csv";//
            string strPath = HttpContext.Current.Server.MapPath(strVPath);
            File.WriteAllText(strPath, str.ToString());
            /* HttpContext.Current.Response.Redirect(strVPath); */
            /*#CC03 Code Commented*/
            HttpContext.Current.Response.Redirect(strVPath, false); /*#CC03 Code Added*/
        }
        /*#CC05 start*/
        public bool SaveCSVFilePath(DataTable data, string strVPath, bool isheader)
        {
            StringBuilder str = new StringBuilder();
            Int32 Howmanyrows = 0;
            Int32 counter = 0;
            Howmanyrows = data.Rows.Count;
            if (isheader == true)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    str.Append(data.Columns[i].ColumnName.ToString() + ",");
                }
                str.Replace(",", Environment.NewLine, str.Length - 1, 1);
            }
            foreach (DataRow dr in data.Rows)
            {
                counter = counter + 1;
                foreach (object field in dr.ItemArray)
                {
                    str.Append(field.ToString() + ",");
                }
                if (Howmanyrows == counter)/*Now it is last row of the data*/
                    str.Replace(",", "", str.Length - 1, 1);
                else
                    str.Replace(",", Environment.NewLine, str.Length - 1, 1);
            }

            //string strVPath = "~/UploadDownload/UploadPersistent/SAPFOLDER/" + fileName + System.DateTime.Now.Ticks.ToString() + ".csv";//
            string strPath = HttpContext.Current.Server.MapPath(strVPath);
            File.WriteAllText(strPath, str.ToString());
            return true;
        }/*#CC05 end*/

        public int CountExcelRows(string filename)
        {
            int rowCount = 0;
            DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadSheet = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(filename, true);
            using (spreadSheet)
            {
                WorkbookPart workbook = spreadSheet.WorkbookPart;
                IEnumerable<Sheet> sheets = workbook.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                //for (int i = 0; i < sheets.Count(); i++)
                //{
                string rId = sheets.ElementAt(0).Id;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheet.WorkbookPart.GetPartById(rId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();
                rowCount = rows.Count();
                //}
            }
            return rowCount;

        }

        public DataSet ImportExcelFile(string filelocation)
        {
            string excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filelocation + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=YES;\"";
            //OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter("Select * FROM [Sheet1$] ", excelConnectionString);
            //DataSet dataSet = new DataSet();
            //oleDbDataAdapter.Fill(dataSet);
            //return dataSet;


            OleDbConnection dbCon = new OleDbConnection(excelConnectionString);

            dbCon.Open();


            // Get All Sheets Name

            DataTable dtSheetName = dbCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            // Retrive the Data by Sheetwise



            DataSet dsOutput = new DataSet();

            for (int nCount = 0; nCount < dtSheetName.Rows.Count; nCount++)
            {

                string sSheetName = dtSheetName.Rows[nCount]["TABLE_NAME"].ToString();
                if (sSheetName.ToLower() != "help$")
                {
                    string sQuery = "Select * From [" + sSheetName + "]";

                    OleDbCommand dbCmd = new OleDbCommand(sQuery, dbCon);

                    OleDbDataAdapter dbDa = new OleDbDataAdapter(dbCmd);

                    DataTable dtData = new DataTable();

                    dbDa.Fill(dtData);

                    dsOutput.Tables.Add(dtData);
                }

            }

            dbCon.Close();


            return dsOutput;


        }

    }

}

