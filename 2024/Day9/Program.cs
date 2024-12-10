using AdventOfCode.Helpers;

using System.Text;

var solution = new SolutionDay9("input.txt");
solution.PrintSolutions();

//Console.WriteLine($"Part 2: {solution.Part2()}");

public class SolutionDay9 : AbstractSolution
{
    public const char FREE = '.';

    public SolutionDay9() : base() { }
    public SolutionDay9(string filepath) : base(filepath) { }

    public override long Part1()
    {
        var dataAtDisk = GetDiskLayout();
        Compress(dataAtDisk);
        return GetCheckSum(dataAtDisk);
    }

    public override long Part2()
    {
        var dataAtDisk = GetDiskLayout();
        CompressOnlyFit(dataAtDisk);
        return GetCheckSum(dataAtDisk);
    }

    private static void PrintDataAtDisk(List<File> dataAtDisk)
    {
        return;
        foreach (var file in dataAtDisk)
        {
            for (int i = 0; i < file.Size; i++)
            {
                Console.Write(file.Id);
            }
            for (int i = 0; i < file.Free; i++)
            {
                Console.Write(FREE);
            }
        }
        Console.WriteLine();
    }

    private static long GetCheckSum(List<File> files)
    {
        var checksum = 0L;
        var position = 0;
        for (int i = 0; i < files.Count; i++)
        {
            for (int j = 0; j < files[i].Size; j++)
            {
                checksum += position * long.Parse(files[i].Id);
                position++;
            }
            for (int j = 0; j < files[i].Free; j++)
            {
                position++;
            }
        }
        return checksum;
    }

    private static void Compress(List<File> dataAtDisk)
    {
        while (true)
        {
            var lastFreeFileIndex = dataAtDisk.FindIndex(f => f.Free > 0);
            var lastSizeFileIndex = dataAtDisk.FindLastIndex(f => f.Size > 0);
            if (lastFreeFileIndex >= lastSizeFileIndex)
            {
                break;
            }

            var lastFile = dataAtDisk[lastSizeFileIndex];
            for (int i = 0; i < dataAtDisk.Count; i++)
            {
                if (dataAtDisk[i].Free > 0)
                {
                    if (dataAtDisk[i].Free >= lastFile.Size)
                    {
                        dataAtDisk.Insert(i + 1, new File
                        {
                            Id = lastFile.Id,
                            Size = lastFile.Size,
                            Free = dataAtDisk[i].Free - lastFile.Size
                        });
                        dataAtDisk[i].Free = 0;
                        lastFile.Free += lastFile.Size;
                        lastFile.Size = 0;
                        break;
                    }
                    else
                    {
                        dataAtDisk.Insert(i + 1, new File
                        {
                            Id = lastFile.Id,
                            Size = dataAtDisk[i].Free,
                            Free = 0
                        });
                        lastFile.Size -= dataAtDisk[i].Free;
                        lastFile.Free += dataAtDisk[i].Free;
                        dataAtDisk[i].Free = 0;
                    }
                }
            }
            PrintDataAtDisk(dataAtDisk);
        }
    }

    private static void CompressOnlyFit(List<File> dataAtDisk)
    {
        var fileToMove = dataAtDisk.Count - 1;
        for (int i = fileToMove; i >= 0; i--)
        {
            var activeFile = dataAtDisk[i];
            for (int j = 0; j < i; j++)
            {
                if (dataAtDisk[j].Free >= activeFile.Size)
                {
                    dataAtDisk.Insert(j + 1, new File
                    {
                        Id = activeFile.Id,
                        Size = activeFile.Size,
                        Free = dataAtDisk[j].Free - activeFile.Size
                    });
                    dataAtDisk[j].Free = 0;
                    activeFile.Free += activeFile.Size;
                    activeFile.Size = 0;
                    break;
                }
            }
            PrintDataAtDisk(dataAtDisk);
        }
    }


    private List<File> GetDiskLayout()
    {
        var files = new List<File>();
        var fileId = 0;
        for (int i = 0; i < _input.Length; i++)
        {
            var file = new File
            {
                Id = fileId++.ToString(),
                Size = long.Parse(_input[i].ToString()),
            };
            if (++i < _input.Length)
            {
                file.Free = long.Parse(_input[i].ToString());
            }
            files.Add(file);
        }
        return files;
    }

    public record File
    {
        public string Id { get; set; }
        public long Size { get; set; }
        public long Free { get; set; }
    }
}
