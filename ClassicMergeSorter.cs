using System.Numerics;

namespace Lab1;

public class ClassicMergeSorter : MergeSorter
{
    protected override void Divide(string initialFile, long partSize, long elemNum, string firstTemporaryFile = "temp1",
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

        while (!EndOfStream(binaryReader))
        {
            binaryWriters[(int)(counter/partSize%2)].Write(binaryReader.ReadInt32());
            counter++;
        }
        
        binaryReader.Close();
        foreach (BinaryWriter writer in binaryWriters)
        {
            writer.Close();
        }
        
    }

    protected override void Merge(string resultingFile, long partSize, long numberOfElements, string firstTemporaryFile = "temp1",
        string secondTemporaryFile = "temp2")
    {
        BinaryWriter binaryWriter = new BinaryWriter(new FileStream(resultingFile,
            File.Exists(resultingFile) ? FileMode.Open : FileMode.Create));
        BinaryReader firstBinaryReader = new BinaryReader(new FileStream(firstTemporaryFile, FileMode.Open));
        BinaryReader secondBinaryReader = new BinaryReader(new FileStream(secondTemporaryFile, FileMode.Open));

        int a, b, ctrA, ctrB;

        while (!EndOfStream(secondBinaryReader))
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
                    else
                    {
                        binaryWriter.Write(b);
                        ctrB++;
                        while (ctrB<partSize)
                        {
                            binaryWriter.Write(secondBinaryReader.ReadInt32());
                            ctrB++;
                        }
                        break;
                    }
                }
                else
                {
                    binaryWriter.Write(b);
                    ctrB++;
                    if (ctrB < partSize && !EndOfStream(secondBinaryReader))
                    {
                        b = secondBinaryReader.ReadInt32();
                    }
                    else
                    {
                        binaryWriter.Write(a);
                        ctrA++;
                        while (ctrA < partSize)
                        {
                            binaryWriter.Write(firstBinaryReader.ReadInt32());
                            ctrA++;
                        }
                        break;
                    }
                }
            }
        }

        while (!EndOfStream(firstBinaryReader))
        {
            binaryWriter.Write(firstBinaryReader.ReadInt32());
        }
        
        binaryWriter.Close();
        firstBinaryReader.Close();
        secondBinaryReader.Close();
        File.Delete(firstTemporaryFile);
        File.Delete(secondTemporaryFile);
    }

    public override void Sort(string filename, long elemNumber)
    {
        for (long i = 1; i < elemNumber; i*=2)
        {
            Divide(filename, i, elemNumber);
            Merge(filename, i, elemNumber);
        }
    }
}