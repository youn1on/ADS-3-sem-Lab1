using System;
using System.Diagnostics;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            FileGenerator.GenerateFile(1);
            Console.WriteLine($"[File generated: {sw.ElapsedMilliseconds} ms]");
            sw.Restart();
            BinaryToCsvConverter.ToCsv("generated", "input.csv");
            Console.WriteLine($"[Input file converted: {sw.ElapsedMilliseconds} ms]");
            sw.Restart();
            string file = InputOperations.GetFilepath();
            Console.WriteLine("[Sorting started]");
            sw.Restart();
            MergeSorter sorter = new ClassicMergeSorter();
            sorter.Sort(file, InputOperations.InputtedFileSize);
            Console.WriteLine($"[File sorted: {sw.ElapsedMilliseconds} ms]");
            sw.Restart();
            BinaryToCsvConverter.ToCsv("generated", "output.csv");
            Console.WriteLine($"[Output file converted: {sw.ElapsedMilliseconds} ms]");
        }
    }
}