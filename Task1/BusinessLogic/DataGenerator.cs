using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IDataGenerator
    {
        RowRepresentation GetRow();
    }

    /// <summary>
    /// Generates random row by specified template.
    /// </summary>
    public class DataGenerator : IDataGenerator
    {
        private static Random random = new Random();

        private int lastYearsNumber = 5;
        public int LastYearsNumber
        {
            get
            {
                return lastYearsNumber;
            }
            set
            {
                lastYearsNumber = value;
                TotalDaysAmount = (DateTime.Today - DateTime.Today.AddYears(-lastYearsNumber)).Days;
            }
        }

        private int precision = 8;
        /// <summary>
        /// Gets or Sets precision of the double number.
        /// </summary>
        public int Precision
        {
            get
            {
                return precision;
            }
            set
            {
                if (value < 0 || value > 15)
                {
                    throw new ArgumentException("Precision must be less than 16 and greater than 0.");
                }

                precision = value;
            }
        }

        public int LatinSequenceLength { get; set; } = 10;
        public int CyrillicSequenceLength { get; set; } = 10;
        public int IntFloor { get; set; } = 1;
        public int IntCeil { get; set; } = 100000000;
        public int FloatFloor { get; set; } = 1;
        public int FloatCeil { get; set; } = 20;

        public char[] LatinSymbols { get; set; } = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        public char[] CyrillicSymbols { get; set; } = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я' };

        private int TotalDaysAmount;

        public DataGenerator()
        {
            LastYearsNumber = lastYearsNumber;
        }

        /// <summary>
        /// Generates row by next template: "{date}||{latinSymbos}||{cyrillicSymbos}||{int}||{float}"
        /// </summary>
        /// <returns>Generated row.</returns>
        public RowRepresentation GetRow()
        {
            string latinSequence = GetRandomSequence(LatinSymbols, LatinSequenceLength);
            string cyrillicSequence = GetRandomSequence(CyrillicSymbols, CyrillicSequenceLength);

            return new RowRepresentation() {
                Date = GetDate(),
                LatinSequence = latinSequence,
                CyrillicSequence = cyrillicSequence,
                IntegerNumber = GetInt(),
                FloatNumber = GetFloat()
            };
        }

        /// <summary>
        /// Generates random int number from specified range.
        /// </summary>
        /// <returns></returns>
        private int GetInt()
        {
            if (IntFloor > IntCeil)
            {
                throw new ArgumentOutOfRangeException("IntCeil can't be less than IntFloor");
            }

            return random.Next(IntFloor, IntCeil + 1);
        }

        /// <summary>
        /// Generates random date in specified year range.
        /// </summary>
        /// <returns>Random date.</returns>
        private DateTime GetDate()
        {
            return DateTime.Today.AddDays(-random.Next(0, TotalDaysAmount + 1));
        }

        /// <summary>
        /// Generates random sequence of specified symbols.
        /// </summary>
        /// <param name="symbols">Specified symbols.</param>
        /// <param name="newSequenceLength">Length of the new sequence.</param>
        /// <returns>Random sequence made of specified symbols.</returns>
        private string GetRandomSequence(char[] symbols, int newSequenceLength)
        {
            char[] newSequence = new char[newSequenceLength];

            for (int i = 0; i < newSequence.Length; i++)
            {
                newSequence[i] = symbols[random.Next(0, symbols.Length)];
            }

            return new string(newSequence);
        }

        /// <summary>
        /// Generates random float number with specified precision.
        /// </summary>
        /// <returns>Float number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when FloatCeil < FloatFloor.</exception>
        private double GetFloat()
        {
            if (FloatFloor > FloatCeil)
            {
                throw new ArgumentOutOfRangeException("IntCeil can't be less than IntFloor");
            }

            return Math.Round(random.NextDouble() + random.Next(FloatFloor, FloatCeil), Precision);
        }
    }
}
