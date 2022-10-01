using System.Text;

namespace Lab1;

public class FileGenerator
{
    public static void GenerateFile(int sizeInMb, string filename = "generated")
    {
        long numberOfElements = 1048576 * sizeInMb / 4;
        BinaryWriter binaryWriter = new BinaryWriter(new FileStream(filename, FileMode.Create));
        Random rand = new Random();
        for (int i = 0; i < numberOfElements; i++)
        {
            binaryWriter.Write(rand.Next(short.MinValue, short.MaxValue));
        }
        binaryWriter.Close();
    }
}