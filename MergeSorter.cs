namespace Lab1;

public abstract class MergeSorter
{
    protected static bool EndOfStream(BinaryReader binaryReader) =>
        binaryReader.BaseStream.Position == binaryReader.BaseStream.Length;
    
    protected virtual void Merge(string resultingFile, long partSize, long numberOfElements, string firstTemporaryFile = "temp1",
        string secondTemporaryFile = "temp2") {}
    
    protected virtual void Divide(string initialFile, long partSize, long elemNum, string firstTemporaryFile = "temp1",
        string secondTemporaryFile = "temp2") {}
    
    public virtual void Sort(string filename, long elemNumber){}
}