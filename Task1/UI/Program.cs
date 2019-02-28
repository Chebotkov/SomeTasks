using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using UI.Data;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            DataGenerator generator = new DataGenerator();
            FileWorker fileWorker = new FileWorker(generator);
            DBWorker dbWorker = new DBWorker(); 
            
            //Generating files
            fileWorker.GenerateFiles("D:\\Files");

            //Merging files into one
            Console.WriteLine(fileWorker.MergeFiles("D:\\Files", null, "ПаШа") + " strings were deleted.");


            fileWorker.ProcessInfo += FileWorker_ProcessInfo;
            //Saving data to DB
            fileWorker.SaveData("D:\\Files\\Result.txt", dbWorker);

            Console.WriteLine("Sum is: " + dbWorker.GetSumFromIntColumn());
            Console.WriteLine("Median is: " + dbWorker.GetMedian());
        }

        private static void FileWorker_ProcessInfo(int rowsProcessed, int rowsLeft)
        {
            Console.WriteLine("Writed strings count: {0}, left strings count: {1}", rowsProcessed, rowsLeft);
        }
    }
}
