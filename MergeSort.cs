namespace Lab1;

public class MergeSort
{
    public static int Divide(string initialFile, int partSize, string firstTemporaryFile = "temp1",
        string secondTemporaryFile = "temp2")
    {
        BinaryReader binaryReader = new BinaryReader(new FileStream(initialFile, FileMode.Open));
        BinaryWriter[] binaryWriters =
        {
            new BinaryWriter(new FileStream(firstTemporaryFile,
                File.Exists(firstTemporaryFile) ? FileMode.Open : FileMode.Create)),
            new BinaryWriter(new FileStream(secondTemporaryFile,
                File.Exists(secondTemporaryFile) ? FileMode.Open : FileMode.Create))
        };

        int counter = 0;

        while (binaryReader.PeekChar() > -1)
        {
            binaryWriters[counter/partSize%2].Write(binaryReader.ReadInt32());
            counter++;
        }
        
        binaryReader.Close();
        foreach (BinaryWriter writer in binaryWriters)
        {
            writer.Close();
        }

        return counter;
    }

    public static void Merge(string resultingFile, int partSize, int numberOfElements, string firstTemporaryFile = "temp1",
        string secondTemporaryFile = "temp2")
    {
        BinaryWriter binaryWriter = new BinaryWriter(new FileStream(resultingFile,
            File.Exists(resultingFile) ? FileMode.Open : FileMode.Create));
        BinaryReader firstBinaryReader = new BinaryReader(new FileStream(firstTemporaryFile, FileMode.Open));
        BinaryReader secondBinaryReader = new BinaryReader(new FileStream(secondTemporaryFile, FileMode.Open));

        int a, b, ctrA, ctrB;

        while (secondBinaryReader.PeekChar() > -1)
        {
            ctrA = 0;
            ctrB = 0;
            a = firstBinaryReader.ReadInt32();
            b = secondBinaryReader.ReadInt32();
            while (true)
            {
                if (a <= b)
                {
                    binaryWriter.Write(a);
                    ctrA++;
                    if (ctrA < partSize)
                    {
                        a = firstBinaryReader.ReadInt32();
                    }
                    else break;
                }
                else
                {
                    binaryWriter.Write(b);
                    ctrB++;
                    if (ctrB < partSize && secondBinaryReader.PeekChar() > -1)
                    {
                        b = secondBinaryReader.ReadInt32();
                    }
                    else break;
                }
            }

            for (int i = ctrA; i < partSize; i++)
            {
                binaryWriter.Write(firstBinaryReader.ReadInt32());
            }
            
            for (int i = ctrB; i < partSize && secondBinaryReader.PeekChar() > -1; i++)
            {
                binaryWriter.Write(secondBinaryReader.ReadInt32());
            }
        }

        while (firstBinaryReader.PeekChar() > -1)
        {
            binaryWriter.Write(firstBinaryReader.ReadInt32());
        }
        
        binaryWriter.Close();
        firstBinaryReader.Close();
        secondBinaryReader.Close();

    }

    public static void Sort(string filename, int elemNumber)
    {
        for (int i = 1; i < elemNumber; i*=2)
        {
            Divide(filename, i);
            Merge(filename, i, elemNumber);
        }
    }
}