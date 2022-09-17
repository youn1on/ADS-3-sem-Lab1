namespace Lab1;

public class InputOperations
{
    public static int InputtedFileSize;
    public static string GetFilepath()
    {
        while (true)
        {
            Console.WriteLine("Enter your filepath");
            string filepath = Console.ReadLine();
            if (File.Exists(filepath)) return filepath;
            Console.WriteLine("Incorrect filepath");
        }
    }

    private static bool ValidateFile()
    {
        return true;
    }
}