// See https://aka.ms/new-console-template for more information
using System.Numerics;

Console.WriteLine("Solution 1: " + Area(Steps1(File.ReadAllLines("input.txt"))));
Console.WriteLine("Solution 2: " + Area(Steps2(File.ReadAllLines("input.txt"))));


static IEnumerable<Complex> Steps1(string[] input) =>
      from line in input
      let parts = line.Split(' ')
      let dir = parts[0] switch
      {
          "R" => Complex.One,
          "U" => -Complex.ImaginaryOne,
          "L" => -Complex.One,
          "D" => Complex.ImaginaryOne,
          _ => throw new Exception()
      }
      let dist = int.Parse(parts[1])
      select dir * dist;

static IEnumerable<Complex> Steps2(string[] input) =>
      from line in input
      let hex = line.Split(' ')[2]
      let dir = hex[7] switch
      {
          '0' => Complex.One,
          '1' => -Complex.ImaginaryOne,
          '2' => -Complex.One,
          '3' => Complex.ImaginaryOne,
          _ => throw new Exception()
      }
      let dist = Convert.ToInt32(hex[2..7], 16)
      select dir * dist;

static double Area(IEnumerable<Complex> steps)
{
    var vertices = Vertices(steps).ToList();
    var shiftedVertices = vertices.Skip(1).Append(vertices[0]);
    var shoelaces =
        from points in vertices.Zip(shiftedVertices)
        let p1 = points.First
        let p2 = points.Second
        select p1.Real * p2.Imaginary - p1.Imaginary * p2.Real;
    var area = Math.Abs(shoelaces.Sum()) / 2;
    var boundary = steps.Select(x => x.Magnitude).Sum();
    var interior = area - boundary / 2 + 1;
    return boundary + interior;
}

static IEnumerable<Complex> Vertices(IEnumerable<Complex> steps)
{
    var pos = Complex.Zero;
    foreach (var step in steps)
    {
        pos += step;
        yield return pos;
    }
}