using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TestServer.Models;

namespace TestServer.Models
{
    /// <summary>
    /// Works with ExcelFileDB database.
    /// </summary>
    public static class DBServant
    {
        /// <summary>
        /// Gets file model by file name.
        /// </summary>
        /// <param name="fileName">Specified file name.</param>
        /// <returns>File model.</returns>
        public static File GetFile(string fileName)
        {
            File file = null;
            using (ExcelFileDBContext context = new ExcelFileDBContext())
            {
                file = context.File.Where(f => f.Name == fileName).FirstOrDefault();
            }

            return file;
        }

        /// <summary>
        /// Gets list of classes that belongs to specified file.
        /// </summary>
        /// <param name="fileId">Id of the specified file.</param>
        /// <returns>List of classes.</returns>
        public static IEnumerable<Class> GetClasses(int fileId)
        {
            IEnumerable <Class> classes = null;

            using (ExcelFileDBContext context = new ExcelFileDBContext())
            {
                classes = context.Class.Where(c => c.FileId == fileId);
                foreach (Class @class in classes)
                {
                    context.Entry(@class).Collection("Balances").Load();
                }

                classes = classes.ToList();
            }

            return classes;
        }

        /// <summary>
        /// Gets sums of all columns from all classes.
        /// </summary>
        /// <param name="fileId">Specified file id</param>
        /// <returns>All sums by classes.</returns>
        public static IEnumerable<BalanceNumber> GetSumByClass(int fileId)
        {
            Queue<BalanceNumber> result = new Queue<BalanceNumber>();

            using (ExcelFileDBContext context = new ExcelFileDBContext())
            {
                var classes = context.Class.Where(c => c.FileId == fileId).Select(c => new { c.ClassId, });

                foreach (var temp in classes)
                {
                    result.Enqueue(GetTotalSum("[dbo].[SumByClass]", temp.ClassId));
                }
            }

            return result;
        }

        /// <summary>
        /// Gets total sum of all columns.
        /// </summary>
        /// <returns>Total sum of all columns</returns>
        public static BalanceNumber GetTotalSum()
        {
            return GetTotalSum("[dbo].[TotalSum]");
        }

        /// <summary>
        /// Executes stored in data base procedure.
        /// </summary>
        /// <param name="procedureName">Procedure name.</param>
        /// <param name="classId">Optional input parameter.</param>
        /// <returns>Result of the executed procedure.</returns>
        private static BalanceNumber GetTotalSum(string procedureName, int classId = -1)
        {
            BalanceNumber balance = new BalanceNumber();

            using (ExcelFileDBContext context = new ExcelFileDBContext())
            {
                try
                {
                    SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString);

                    SqlCommand command = new SqlCommand(procedureName, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var AB = new SqlParameter
                    {
                        ParameterName = "@ABSum",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Decimal,
                        Scale = 2
                    };

                    var PB = new SqlParameter
                    {
                        ParameterName = "@PBSum",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Decimal,
                        Scale = 2
                    };

                    var TD = new SqlParameter
                    {
                        ParameterName = "@TDSum",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Decimal,
                        Scale = 2
                    };

                    var TL = new SqlParameter
                    {
                        ParameterName = "@TLSum",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Decimal,
                        Scale = 2
                    };

                    var AOB = new SqlParameter
                    {
                        ParameterName = "@AOBSum",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Decimal,
                        Scale = 2
                    };

                    var POB = new SqlParameter
                    {
                        ParameterName = "@POBSum",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Decimal,
                        Scale = 2
                    };

                    if (classId > 0)
                    {
                        balance.ClassId = classId;

                        var input = new SqlParameter
                        {
                            ParameterName = "@classId",
                            Direction = System.Data.ParameterDirection.Input,
                            SqlDbType = System.Data.SqlDbType.Int,
                            SqlValue = classId,
                        };

                        command.Parameters.Add(input);
                    }

                    command.Parameters.Add(AB);
                    command.Parameters.Add(PB);
                    command.Parameters.Add(TD);
                    command.Parameters.Add(TL);
                    command.Parameters.Add(AOB);
                    command.Parameters.Add(POB);

                    connection.Open();
                    command.ExecuteNonQuery();

                    balance.AssetBalance = Convert.ToDecimal(command.Parameters["@ABSum"].Value);
                    balance.PassiveBalance = Convert.ToDecimal(command.Parameters["@PBSum"].Value);
                    balance.TurnoverDebit = Convert.ToDecimal(command.Parameters["@TDSum"].Value);
                    balance.TurnoverLoan = Convert.ToDecimal(command.Parameters["@TLSum"].Value);
                    balance.AssetOutgoingBalance = Convert.ToDecimal(command.Parameters["@AOBSum"].Value);
                    balance.PassiveOutgoingBalance = Convert.ToDecimal(command.Parameters["@POBSum"].Value);

                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return balance;
        }
    }
}