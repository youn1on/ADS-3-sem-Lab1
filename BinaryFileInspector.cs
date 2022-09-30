using System.ComponentModel;

namespace Lab1;

public class BinaryFileInspector
{
    public static void Inspect(string filename, string outputFile, int inspectedElemNumber)
    {
        BinaryReader binaryReader = new BinaryReader(new FileStream(filename, FileMode.Open));
        StreamWriter streamWriter = new StreamWriter(outputFile);

        FileInfo fileInfo = new FileInfo(filename);
        long fileSize = fileInfo.Length;
        if (fileSize > 0)
        {
            if (fileSize <= inspectedElemNumber * 4 * 2)
            {
                byte[] binData = binaryReader.ReadBytes((int)fileSize);
                streamWriter.Write(BitConverter.ToInt32(binData[0..4]));
                for (int i = 1; i < binData.Length / 4; i++)
                    streamWriter.Write(", " + BitConverter.ToInt32(binData[(i * 4)..((i + 1) * 4)]));
            }
            else
            {
                byte[] binData = binaryReader.ReadBytes(inspectedElemNumber * 4);
                streamWriter.Write(BitConverter.ToInt32(binData[0..4]));
                for (int i = 1; i < inspectedElemNumber; i++)
                    streamWriter.Write(", " + BitConverter.ToInt32(binData[(i * 4)..((i + 1) * 4)]));

                streamWriter.Write("\n ... \n");
                binaryReader.BaseStream.Seek(fileSize - inspectedElemNumber * 4, SeekOrigin.Begin);

                binData = binaryReader.ReadBytes(inspectedElemNumber * 4);
                streamWriter.Write(BitConverter.ToInt32(binData[0..4]));
                for (int i = 1; i < inspectedElemNumber; i++)
                    streamWriter.Write(", " + BitConverter.ToInt32(binData[(i * 4)..((i + 1) * 4)]));
            }
        }

        binaryReader.Close();
        streamWriter.Close();
    }
}