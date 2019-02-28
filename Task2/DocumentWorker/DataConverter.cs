using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentWorker
{
    public class DataConverter
    {
        public string ContentDivider { get; set; } = "класс";
        public int ColumnsCount { get; set; } = 7;

        public void ConvertData(object[][] data)
        {
            object[] currentArray = null;

            for (int i = 0; i < data.Length; i++)
            {
                currentArray = data[i];

                if (currentArray.Length == 1 && currentArray[0].ToString().ToLower().Contains(ContentDivider))
                {
                    Console.WriteLine(currentArray[0]);
                }

                if (currentArray.Length == ColumnsCount)
                {
                    for (int j = 0; j < currentArray.Length; j++)
                    {
                        Console.Write(currentArray[j].ToString() + " || " );
                    }

                    Console.WriteLine();
                }

            }
        }
    }
}
