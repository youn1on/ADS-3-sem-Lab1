namespace Lab1;

public class FileGenerator
{
    public static void GenerateFile(int sizeInMb, string filename = "generated")
    {
        long numberOfElementsInOneMb = 1048576 / 4;
        BinaryWriter binaryWriter = new BinaryWriter(new FileStream(filename, FileMode.Create));
        Random rand = new Random();
        for (int i = 0; i < sizeInMb; i++)
        {
            for (int j = 0; j < numberOfElementsInOneMb; j++)
            {
                binaryWriter.Write(rand.Next(int.MinValue, int.MaxValue));
            }
        }

        binaryWriter.Close();
    }
}