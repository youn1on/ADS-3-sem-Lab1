using System.Numerics;

namespace Lab1;

public class InputOperations
{
    public static long InputtedFileSize;
    public static string GetFilepath()
    {
        while (true)
        {
            Console.WriteLine("Enter your filepath");
            string filepath = Console.ReadLine();
            if (File.Exists(filepath) && ValidateFile(filepath)) return filepath;
            Console.WriteLine("Incorrect filepath");
        }
    }

    private static bool ValidateFile(string filename)
    {
        FileInfo info = new FileInfo(filename);
        InputtedFileSize = info.Length / 4;
        return info.Length % 4 == 0;
    }
}