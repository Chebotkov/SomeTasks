using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentWorker;
using TestServer.Models;

namespace TestServer.Logic
{
    /// <summary>
    /// Works with database.
    /// </summary>
    public class DataToDB
    {
        /// <summary>
        /// Saves data from specified file to database.
        /// </summary>
        /// <param name="fileName">Specified file.</param>
        /// <returns>True if file was saved to database. False - if wasn't.</returns>
        public bool SaveDataToDB(string fileName)
        {
            using (ExcelFileDBContext context = new ExcelFileDBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    File file = new File();
                    Class @class = null;
                    BalanceNumber balance = new BalanceNumber();
                    int currentClassId = 0;
                    int currentFileId = 0;

                    ExcelReader excelReader = new ExcelReader();

                    object[][] rows = excelReader.LoadExcelRows(fileName);

                    try
                    {
                        file.BankName = rows[0][0].ToString();
                        string clearFileName = file.Name = FileInfo.GetFileName(fileName);
                        GetDates(rows[2][0].ToString(), out DateTime fromDate, out DateTime toDate);
                        file.FromDate = fromDate;
                        file.ToDate = toDate;

                        context.File.Add(file);
                        context.SaveChanges();

                        currentFileId = file.FileId;

                        for (int i = 8; i < rows.Length - 2; i++)
                        {
                            if (rows[i].Length == 1)
                            {
                                @class = new Class();
                                @class.Name = rows[i][0].ToString();
                                @class.FileId = currentFileId;

                                context.Class.Add(@class);
                                context.SaveChanges();

                                currentClassId = @class.ClassId;
                            }

                            if (@class != null)
                            {
                                if (IsBalance(rows[i]))
                                {
                                    context.BalanceNumber.Add(GetBalance(rows[i], currentClassId));
                                }
                            }
                        }

                        context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        //If there was some errors it's necessary to rollback transaction.
                        transaction.Rollback();
                        return false;
                    }
                    
                    transaction.Commit();
                }
            }

            return true;
        }

        /// <summary>
        /// Gets dates from row.
        /// </summary>
        /// <param name="row">Specified row.</param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        private void GetDates(string row, out DateTime fromDate, out DateTime toDate)
        {
            int index = row.IndexOf("с");
            fromDate = DateTime.Parse(row.Substring(index + 2, 10));
            toDate = DateTime.Parse(row.Substring(index + 16));
        }

        /// <summary>
        /// Returns BalanceNumber representation from specified cells.
        /// </summary>
        /// <param name="cells">Array of cells.</param>
        /// <param name="classId">Id of the class that BalanceNumber belongs to.</param>
        /// <returns>Instance of the parsed BalanceNumber.</returns>
        private BalanceNumber GetBalance(object[] cells, int classId)
        {
            BalanceNumber balance = new BalanceNumber();
            balance.ClassId = classId;
            balance.BalanceId = Int32.Parse(cells[0].ToString());
            balance.AssetBalance = Decimal.Parse(cells[1].ToString());
            balance.PassiveBalance = Decimal.Parse(cells[2].ToString());
            balance.TurnoverDebit = Decimal.Parse(cells[3].ToString());
            balance.TurnoverLoan = Decimal.Parse(cells[4].ToString());
            balance.AssetOutgoingBalance = Decimal.Parse(cells[5].ToString());
            balance.PassiveOutgoingBalance = Decimal.Parse(cells[6].ToString());

            return balance;
        }

        /// <summary>
        /// Checks if row of cells is balance representation.
        /// </summary>
        /// <param name="cells">Specified cells.</param>
        /// <returns>True if balance. False - isn't.</returns>
        private bool IsBalance(object[] cells)
        {
            if (cells.Length == 7 && Int32.TryParse(cells[0].ToString(), out int num))
            {
                if (num > 100)
                {
                    return true;
                }
            }

            return false;
        }
    }
}