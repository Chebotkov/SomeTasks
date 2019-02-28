using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IDataSaver
    {
        void SaveRow(RowRepresentation row);
    }

    /// <summary>
    /// Represents rows from the file.
    /// </summary>
    public struct RowRepresentation
    {
        public DateTime Date { get; set; }
        public string LatinSequence { get; set; }
        public string CyrillicSequence { get; set; }
        public int IntegerNumber { get; set; }
        public double FloatNumber { get; set; }
        
        /// <summary>
        /// Parses string row to the RowRepresentation.
        /// </summary>
        /// <param name="row">Specified row.</param>
        /// <returns>Parsed result.</returns>
        public static RowRepresentation ParseToRowRepresentation(string row)
        {
            RowRepresentation rowRepresentation = new RowRepresentation();

            string[] values = row.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            rowRepresentation.Date = DateTime.Parse(values[0]);

            rowRepresentation.LatinSequence = values[1];

            rowRepresentation.CyrillicSequence = values[2]; 

            rowRepresentation.IntegerNumber = Int32.Parse(values[3]);

            rowRepresentation.FloatNumber = float.Parse(values[4]);

            return rowRepresentation;
        }

        public override string ToString()
        {
            return String.Format("{0}||{1}||{2}||{3}||{4}", Date.ToShortDateString(), LatinSequence, CyrillicSequence, IntegerNumber, FloatNumber);
        }
    }
}
