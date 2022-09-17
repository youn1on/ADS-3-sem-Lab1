using System;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            BinaryToCsvConverter.ToCsv("generated", "input.csv");
            string file = InputOperations.GetFilepath();
            MergeSort.Sort(file, InputOperations.InputtedFileSize);
            BinaryToCsvConverter.ToCsv("generated", "output.csv");
        }
    }
}