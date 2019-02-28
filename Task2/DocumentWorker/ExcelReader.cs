using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DocumentWorker
{
    public interface ILogger
    {
        void Log(Exception exception);
        void Log(string message);
    }

    /// <summary>
    /// Works on excel files.
    /// </summary>
    public class ExcelReader
    {
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ExcelReader.
        /// </summary>
        public ExcelReader()
        {

        }

        /// <summary>
        /// Initializes a new instance of the ExcelReader.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="logger"/> is null.</exception>
        public ExcelReader(ILogger logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(String.Format("{0} can't be null", logger));
            }

            Logger = logger;
        }

        /// <summary>
        /// Loads all cells from excel file. It means that there is only one sheet in the file.
        /// </summary>
        /// <param name="excelFilePath">Path to excel file.</param>
        /// <returns>Array of existed cells.</returns>
        public object[][] LoadExcelRows(string excelFilePath)
        {
            Application ExcelObj = null;
            _Workbook excelBook = null;
            object[][] result = null;

            try
            {
                ExcelObj = new Application();
                ExcelObj.DisplayAlerts = false;

                excelBook = ExcelObj.Workbooks.Open(excelFilePath);

                Sheets sheets = excelBook.Sheets;
                int maxNumSheet = sheets.Count;

                Logger?.Log("Sheets count: " + maxNumSheet);
                for (int i = 1; i <= maxNumSheet; i++)
                {
                    var currentSheet = (Worksheet)excelBook.Sheets[i];

                    Range excelRange = currentSheet.UsedRange;

                    int maxColNum = excelRange.Columns.Count;
                    int rowsCount = excelRange.Rows.Count;

                    result = new object[rowsCount][];
                    Logger?.Log("Rows count: " + rowsCount + "; Columns number: " + maxColNum);

                    for (int l = 1; l <= rowsCount; l++)
                    {
                        Range RealExcelRangeLoc = currentSheet.Range[(object)currentSheet.Cells[l, 1], (object)currentSheet.Cells[l, maxColNum]];

                        result[l - 1] = GetClearData((object[,])RealExcelRangeLoc.Value[XlRangeValueDataType.xlRangeValueDefault]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger?.Log(ex);
            }
            finally
            {
                if (excelBook != null)
                {
                    excelBook.Close();
                    Marshal.ReleaseComObject(excelBook);
                }
                if (ExcelObj != null) ExcelObj.Quit();
            }

            return result;
        }
        
        /// <summary>
        /// Deletes all empty "cells".
        /// </summary>
        /// <param name="data">Specified data.</param>
        /// <returns>Clear data.</returns>
        private object[] GetClearData(object[,] data)
        {
            object[] result = null;
            int newDimension = 0;

            if (data != null)
            {
                for (int j = 1; j < data.Length; j++)
                {
                    if (data[1, j] != null)
                    {
                        newDimension++;
                    }
                }

                result = new object[newDimension];

                for (int j = 1, indexInNewDimension = 0; j < data.Length; j++)
                {
                    if (data[1, j] != null)
                    {
                        result[indexInNewDimension] = data[1, j];
                        indexInNewDimension++;
                    }
                }
            }

            return result;
        }
    }
}
