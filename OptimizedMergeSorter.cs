using Microsoft.VisualBasic.FileIO;

namespace Lab1;

public class OptimizedMergeSorter : MergeSorter
{
    private int _maxArraySize = 1024 * 1024 * 50 / 4;
    protected override void Divide(string initialFile, int partSize, long elemNum, string firstTemporaryFile = "temp1",
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

        while ((binaryReader.BaseStream.Length - binaryReader.BaseStream.Position)/4 > _maxArraySize )
        {
            binaryWriters[counter/partSize%2].Write(binaryReader.ReadBytes(_maxArraySize*4*2));
            counter+=_maxArraySize*2;
        }
        binaryWriters[counter/partSize%2].Write(binaryReader.ReadBytes((int)(binaryReader.BaseStream.Length - binaryReader.BaseStream.Position)));
        
        binaryReader.Close();
        foreach (BinaryWriter writer in binaryWriters)
        {
            writer.Close();
        }
    }

    protected override void Merge(string resultingFile, int partSize, int numberOfElements, string firstTemporaryFile = "temp1",
        string secondTemporaryFile = "temp2")
    {
        BinaryWriter binaryWriter = new BinaryWriter(new FileStream(resultingFile,
            File.Exists(resultingFile) ? FileMode.Open : FileMode.Create));
        BinaryReader firstBinaryReader = new BinaryReader(new FileStream(firstTemporaryFile, FileMode.Open));
        BinaryReader secondBinaryReader = new BinaryReader(new FileStream(secondTemporaryFile, FileMode.Open));

        int ctrA, ctrB;
        int[] a, b;

        while (!EndOfStream(secondBinaryReader))
        {
            ctrA = 0;
            ctrB = 0;
            int aIterator = 0, bIterator = 0; 
            a = FillDataSource(firstBinaryReader, partSize);
            b = FillDataSource(secondBinaryReader, partSize);
            while (true)
            {
                if (a[aIterator] <= b[bIterator])
                {
                    binaryWriter.Write(a[aIterator++]);
                    ctrA++;
                    if ( a.Length == aIterator)
                    {
                        if (ctrA < partSize)
                        {
                            a = FillDataSource(firstBinaryReader, partSize - ctrA);
                            aIterator = 0;
                        }
                        else
                        {
                            while (b.Length > bIterator)
                            {
                                binaryWriter.Write(b[bIterator]);
                                ctrB++;
                                bIterator++;
                            }
                            while (ctrB < partSize && !EndOfStream(secondBinaryReader))
                            {
                                b = FillDataSource(secondBinaryReader, partSize-ctrB);
                                bIterator = 0;
                                while (b.Length > bIterator)
                                {
                                    binaryWriter.Write(b[bIterator++]);
                                    ctrB++;
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    binaryWriter.Write(b[bIterator++]);
                    ctrB++;
                    if (b.Length - bIterator == 0)
                    {
                        if (ctrB < partSize && !EndOfStream(secondBinaryReader))
                        {
                            b = FillDataSource(secondBinaryReader, partSize - ctrB);
                            bIterator = 0;
                        }
                        else
                        {
                            while (a.Length > aIterator)
                            {
                                binaryWriter.Write(a[aIterator++]);
                                ctrA++;
                            }
                            while (ctrA < partSize)
                            {
                                a = FillDataSource(firstBinaryReader, partSize - ctrA);
                                aIterator = 0;
                                while (a.Length > aIterator)
                                {
                                    binaryWriter.Write(a[aIterator++]);
                                    ctrA++;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        binaryWriter.Write(
            firstBinaryReader.ReadBytes(
                (int)(firstBinaryReader.BaseStream.Length - firstBinaryReader.BaseStream.Length)));

        binaryWriter.Close();
        firstBinaryReader.Close();
        secondBinaryReader.Close();
        File.Delete(firstTemporaryFile);
        File.Delete(secondTemporaryFile);
        Console.WriteLine($"Iteration with {partSize}-element groups done");
    }

    public override void Sort(string filename, int elemNumber)
    {
        PreSort(filename, elemNumber);
        for (int i = _maxArraySize; i < elemNumber; i*=2)
        {
            Divide(filename, i, elemNumber);
            Merge(filename, i, elemNumber);
        }
    }

    private void PreSort(string filename, int elemNumber)
    {
        BinaryReader binaryReader = new BinaryReader(new FileStream(filename, FileMode.Open));
        BinaryWriter binaryWriter = new BinaryWriter(new FileStream("tmp",
            File.Exists("tmp") ? FileMode.Open : FileMode.Create));

        int[] data;
        byte[] binData;
        for (int i = 0; i < elemNumber / _maxArraySize; i++)
        {
            data = new int[_maxArraySize];
            binData = binaryReader.ReadBytes(_maxArraySize * 4);
            for (int j = 0; j < _maxArraySize; j++)
            {
                data[j] = BitConverter.ToInt32(binData[(j*4)..((j+1)*4)]);
            }
            Array.Sort(data);
            foreach (int item in data) binaryWriter.Write(item);
        }
        data = new int[elemNumber % _maxArraySize];
        binData = binaryReader.ReadBytes((elemNumber % _maxArraySize)*4);
        for (int j = 0; j < elemNumber % _maxArraySize; j++)
        {
            data[j] = BitConverter.ToInt32(binData[(j*4)..((j+1)*4)]);
        }
        Array.Sort(data);
        foreach (int item in data) binaryWriter.Write(item);
        
        binaryReader.Close();
        binaryWriter.Close();
        File.Delete(filename);
        FileSystem.RenameFile("tmp", filename);
        Console.WriteLine("Pre-sorting is done!");
    }

    private int[] FillDataSource(BinaryReader br, int wantedSize)
    {
        int count = (int)(br.BaseStream.Length - br.BaseStream.Position);
        byte[] binData = br.ReadBytes(Math.Min(Math.Min(wantedSize*4, count), _maxArraySize*4));
        int[] data = new int[binData.Length / 4];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = BitConverter.ToInt32(binData[(i*4)..((i+1)*4)]);
        }

        return data;
    }
}