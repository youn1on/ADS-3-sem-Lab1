using System.Text;

namespace Lab1;

public class BinaryToCsvConverter
{
    public static void ToCsv(string filename, string outputFile)
    {
        BinaryReader binaryReader = new BinaryReader(new FileStream(filename, FileMode.Open));
        StreamWriter streamWriter = new StreamWriter(outputFile);
        streamWriter.Write(binaryReader.ReadInt32());
        while (binaryReader.PeekChar() != -1)
        {
            streamWriter.Write(",");
            streamWriter.Write(binaryReader.ReadInt32());
        }
        binaryReader.Close();
        streamWriter.Close();
    }
}