using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;

namespace UI.Data
{
    /// <summary>
    /// Works with DB.
    /// </summary>
    public class DBWorker : IDataSaver
    {
        /// <summary>
        /// Saves rows to DB.
        /// </summary>
        /// <param name="row">Specified row.</param>
        /// <exception cref="Exception">Throws when some exeptions occurs.</exception>
        public void SaveRow(RowRepresentation row)
        {
            try
            {
                using (TestDBContext context = new TestDBContext())
                {
                    RandomRows newRow = new RandomRows();

                    newRow.Date = row.Date;
                    newRow.LatinSequence = row.LatinSequence;
                    newRow.CyrillicSequence = row.CyrillicSequence;
                    newRow.IntegerNumber = row.IntegerNumber;
                    newRow.DoubleNumber = row.FloatNumber;

                    context.RandomRows.Add(newRow);
                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns sum from "IntegerNumber" column using stored procedure in DB.
        /// </summary>
        /// <returns>Sum of integers.</returns>
        /// <exception cref="Exception">Throws when some exeptions occurs.</exception>
        public ulong GetSumFromIntColumn()
        {
            ulong result = 0;

            using (TestDBContext context = new TestDBContext())
            {
                try
                {
                    SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString);

                    SqlCommand command = new SqlCommand("[dbo].[IntSum]", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var parameter = new SqlParameter
                    {
                        ParameterName = "@result",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.BigInt,

                    };

                    command.Parameters.Add(parameter);

                    connection.Open();
                    command.ExecuteNonQuery();

                    result = Convert.ToUInt64(command.Parameters["@result"].Value);

                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns median from "DoubleNumber" column using stored procedure in DB.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Throws when some exeptions occurs.</exception>
        public double GetMedian()
        {
            double result = 0;

            using (TestDBContext context = new TestDBContext())
            {
                try
                {
                    SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString);

                    SqlCommand command = new SqlCommand("[dbo].[Median]", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var parameter = new SqlParameter
                    {
                        ParameterName = "@result",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Float,

                    };

                    command.Parameters.Add(parameter);

                    connection.Open();
                    command.ExecuteNonQuery();

                    result = Convert.ToDouble(command.Parameters["@result"].Value);

                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }
    }
}
