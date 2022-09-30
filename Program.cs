using System;
using System.Diagnostics;

namespace Lab1
{
    class Program
    {
        static void Main()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            FileGenerator.GenerateFile(200);
            Console.WriteLine($"[File generated: {sw.ElapsedMilliseconds} ms]");
            sw.Restart();
            BinaryFileInspector.Inspect("generated", "input.csv", 100);
            Console.WriteLine($"[Input file inspected: {sw.ElapsedMilliseconds} ms]");
            sw.Restart();
            string file = InputOperations.GetFilepath();
            Console.WriteLine("[Sorting started]");
            sw.Restart();
            MergeSorter sorter = new OptimizedMergeSorter();
            sorter.Sort(file, InputOperations.InputtedFileSize);
            Console.WriteLine($"[File sorted: {sw.ElapsedMilliseconds} ms]");
            sw.Restart();
            BinaryFileInspector.Inspect("generated", "output.csv", 100);
            Console.WriteLine($"[Output file inspected: {sw.ElapsedMilliseconds} ms]");
        }
    }
}