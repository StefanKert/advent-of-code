
BenchmarkDotNet v0.14.1-nightly.20241126.197, Windows 11 (10.0.22631.4541/23H2/2023Update/SunValley3)
AMD Ryzen 7 7800X3D 4.20GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.101
  [Host]        : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0      : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  NativeAOT 9.0 : .NET 9.0.0, X64 NativeAOT AVX-512F+CD+BW+DQ+VL+VBMI


 Method       | Job           | Runtime       | Mean            | Error         | StdDev        | Median          |
------------- |-------------- |-------------- |----------------:|--------------:|--------------:|----------------:|
 Day1_PartOne | .NET 9.0      | .NET 9.0      |        79.67 μs |      1.434 μs |      3.324 μs |        78.96 μs |
 Day1_PartTwo | .NET 9.0      | .NET 9.0      |       838.35 μs |     13.122 μs |     12.274 μs |       833.48 μs |
 Day2_PartOne | .NET 9.0      | .NET 9.0      |       190.94 μs |      3.435 μs |      4.219 μs |       190.55 μs |
 Day2_PartTwo | .NET 9.0      | .NET 9.0      |       341.73 μs |      6.692 μs |     10.614 μs |       341.65 μs |
 Day3_PartOne | .NET 9.0      | .NET 9.0      |       138.75 μs |      2.630 μs |      5.313 μs |       137.71 μs |
 Day3_PartTwo | .NET 9.0      | .NET 9.0      |       281.27 μs |      5.403 μs |     11.278 μs |       279.90 μs |
 Day4_PartOne | .NET 9.0      | .NET 9.0      |    39,330.78 μs |    528.436 μs |    494.299 μs |    39,397.98 μs |
 Day4_PartTwo | .NET 9.0      | .NET 9.0      |    13,281.82 μs |    183.774 μs |    171.902 μs |    13,236.51 μs |
 Day5_PartOne | .NET 9.0      | .NET 9.0      |       351.42 μs |      6.931 μs |      8.512 μs |       351.58 μs |
 Day5_PartTwo | .NET 9.0      | .NET 9.0      |       550.63 μs |     10.101 μs |      8.955 μs |       547.67 μs |
 Day6_PartOne | .NET 9.0      | .NET 9.0      |     1,403.82 μs |     10.678 μs |      8.916 μs |     1,403.06 μs |
 Day6_PartTwo | .NET 9.0      | .NET 9.0      |   894,367.55 μs | 17,292.416 μs | 15,329.277 μs |   896,415.85 μs |
 Day7_PartOne | .NET 9.0      | .NET 9.0      |     2,904.41 μs |     88.202 μs |    260.065 μs |     2,808.34 μs |
 Day7_PartTwo | .NET 9.0      | .NET 9.0      |     3,014.50 μs |     86.912 μs |    254.897 μs |     3,001.67 μs |
 Day8_PartOne | .NET 9.0      | .NET 9.0      |    31,704.26 μs |    591.522 μs |    809.682 μs |    31,849.10 μs |
 Day8_PartTwo | .NET 9.0      | .NET 9.0      |   115,120.34 μs |  2,260.163 μs |  3,168.432 μs |   115,847.80 μs |
 Day9_PartOne | .NET 9.0      | .NET 9.0      |    64,959.01 μs |    708.293 μs |    591.456 μs |    65,013.86 μs |
 Day9_PartTwo | .NET 9.0      | .NET 9.0      |    22,285.38 μs |    165.460 μs |    154.772 μs |    22,235.45 μs |
 Day1_PartOne | NativeAOT 9.0 | NativeAOT 9.0 |       115.27 μs |      2.167 μs |      2.027 μs |       115.56 μs |
 Day1_PartTwo | NativeAOT 9.0 | NativeAOT 9.0 |     1,812.00 μs |     24.305 μs |     22.735 μs |     1,823.63 μs |
 Day2_PartOne | NativeAOT 9.0 | NativeAOT 9.0 |       231.97 μs |      4.579 μs |      6.113 μs |       230.44 μs |
 Day2_PartTwo | NativeAOT 9.0 | NativeAOT 9.0 |       465.87 μs |      8.653 μs |      7.671 μs |       467.40 μs |
 Day3_PartOne | NativeAOT 9.0 | NativeAOT 9.0 |       196.55 μs |      3.791 μs |      3.724 μs |       195.78 μs |
 Day3_PartTwo | NativeAOT 9.0 | NativeAOT 9.0 |       391.27 μs |      7.811 μs |      8.995 μs |       389.04 μs |
 Day4_PartOne | NativeAOT 9.0 | NativeAOT 9.0 |    49,546.68 μs |    822.710 μs |    769.564 μs |    49,443.14 μs |
 Day4_PartTwo | NativeAOT 9.0 | NativeAOT 9.0 |    18,654.76 μs |    367.420 μs |    393.135 μs |    18,709.66 μs |
 Day5_PartOne | NativeAOT 9.0 | NativeAOT 9.0 |       452.95 μs |      8.527 μs |      8.375 μs |       451.26 μs |
 Day5_PartTwo | NativeAOT 9.0 | NativeAOT 9.0 |       676.93 μs |     11.230 μs |      9.955 μs |       675.76 μs |
 Day6_PartOne | NativeAOT 9.0 | NativeAOT 9.0 |     1,584.36 μs |     13.652 μs |     12.770 μs |     1,587.76 μs |
 Day6_PartTwo | NativeAOT 9.0 | NativeAOT 9.0 | 1,079,662.24 μs | 21,250.993 μs | 22,738.324 μs | 1,078,758.00 μs |
 Day7_PartOne | NativeAOT 9.0 | NativeAOT 9.0 |     2,998.08 μs |     99.099 μs |    282.734 μs |     3,042.16 μs |
 Day7_PartTwo | NativeAOT 9.0 | NativeAOT 9.0 |     2,960.42 μs |     73.679 μs |    211.398 μs |     2,950.67 μs |
 Day8_PartOne | NativeAOT 9.0 | NativeAOT 9.0 |    39,965.20 μs |    687.724 μs |    574.280 μs |    40,097.35 μs |
 Day8_PartTwo | NativeAOT 9.0 | NativeAOT 9.0 |   150,555.22 μs |  2,321.615 μs |  2,171.641 μs |   149,673.73 μs |
 Day9_PartOne | NativeAOT 9.0 | NativeAOT 9.0 |   108,624.06 μs |  1,126.323 μs |    940.530 μs |   108,399.92 μs |
 Day9_PartTwo | NativeAOT 9.0 | NativeAOT 9.0 |    24,602.46 μs |    486.465 μs |    597.422 μs |    24,603.09 μs |
