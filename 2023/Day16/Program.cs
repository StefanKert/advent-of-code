
using System.Numerics;
using System.Resources;
using System.Text;
using Coor = Coor<int>;

var map = ParseAsArray(SplitBy(File.ReadAllText("input.txt"),Environment.NewLine).ToList(), c => c);

var height = map.Height();
var width = map.Width();

int[] allScores =
[
    .. Enumerable.Range(0, height).Select(row => SimulateRays(new Coor(row, -1), Coor.Right)),
    .. Enumerable.Range(0, height).Select(row => SimulateRays(new Coor(row, width), Coor.Left)),
    .. Enumerable.Range(0, width).Select(col => SimulateRays(new Coor(-1, col), Coor.Down)),
    .. Enumerable.Range(0, width).Select(col => SimulateRays(new Coor(height, col), Coor.Up)),
];

Console.WriteLine($"Part 1: {SimulateRays(new Coor(0, -1), Coor.Right)}");
Console.WriteLine($"Part 2: {allScores.Max()}"); // 7228

static string[] SplitBy(string s, string separator)
        => s.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

static T[,] ParseAsArray<T>(IEnumerable<string> input, Func<char, T> parser)
{
    var list = input.ToList();
    var columnCount = list.First().Length;
    var result = new T[list.Count, columnCount];

    for (int row = 0; row < list.Count; row++)
        for (int column = 0; column < columnCount; column++)
        {
            result[row, column] = parser(list[row][column]);
        }

    return result;
}

int SimulateRays(Coor startPosition, Coor startDirection)
{
    var energized = new bool[map!.Height(), map.Width()];
    var used = new bool[map.Height(), map.Width()];

    var rays = new Queue<Ray>([new Ray(startPosition, startDirection)]);
    int result = 0;

    while (rays.TryDequeue(out var ray))
    {
        var next = ray.Position + ray.Direction;
        if (!next.InBoundsOf(map))
        {
            continue;
        }

        if (!energized.Get(next))
        {
            energized.Set(next, true);
            result++;
        }

        switch (map.Get(next))
        {
            case '.':
                rays.Enqueue(new Ray(next, ray.Direction));
                break;

            case '/':
                rays.Enqueue(new Ray(next, Redirect(ray.Direction,
                    (Coor.Up, Coor.Right),
                    (Coor.Right, Coor.Up),
                    (Coor.Down, Coor.Left),
                    (Coor.Left, Coor.Down))));
                break;

            case '\\':
                rays.Enqueue(new Ray(next, Redirect(ray.Direction,
                    (Coor.Up, Coor.Left),
                    (Coor.Right, Coor.Down),
                    (Coor.Down, Coor.Right),
                    (Coor.Left, Coor.Up))));
                break;

            case '-':
                if (ray.Direction == Coor.Left || ray.Direction == Coor.Right)
                {
                    rays.Enqueue(new Ray(next, ray.Direction));
                }
                else
                {
                    if (used.Get(next))
                    {
                        // Do not spawn a ray we've spanwed before
                        break;
                    }

                    rays.Enqueue(new Ray(next, Coor.Left));
                    rays.Enqueue(new Ray(next, Coor.Right));
                    used.Set(next, true);
                }
                break;

            case '|':
                if (ray.Direction == Coor.Left || ray.Direction == Coor.Right)
                {
                    if (used.Get(next))
                    {
                        // Do not spawn a ray we've spanwed before
                        break;
                    }

                    rays.Enqueue(new Ray(next, Coor.Up));
                    rays.Enqueue(new Ray(next, Coor.Down));
                    used.Set(next, true);
                }
                else
                {
                    rays.Enqueue(new Ray(next, ray.Direction));
                }
                break;
        }
    }

    // map.Print(c => energized.Get(c) ? '#' : null);
    return result;
}

static Coor Redirect(Coor coor, params (Coor, Coor)[] transformations)
    => transformations.First(t => t.Item1 == coor).Item2;

file record struct Ray(Coor Position, Coor Direction);


public record Coor<T>(T Y, T X) where T : INumber<T>
{
    public T Row => Y;
    public T Col => X;

    public static readonly Coor<T> Zero = new(T.Zero, T.Zero);
    public static readonly Coor<T> One = new(T.One, T.One);

    public static readonly Coor<T> Up = new(-T.One, T.Zero);
    public static readonly Coor<T> Down = new(T.One, T.Zero);
    public static readonly Coor<T> Left = new(T.Zero, -T.One);
    public static readonly Coor<T> Right = new(T.Zero, T.One);

    public static Coor<T> operator -(Coor<T> me, Coor<T> other) => new(Y: me.Y - other.Y, X: me.X - other.X);
    public static Coor<T> operator +(Coor<T> me, Coor<T> other) => new(Y: me.Y + other.Y, X: me.X + other.X);

    public static readonly Coor<T>[] Directions =
    [
        Right,
        Down,
        Left,
        Up,
    ];

    public static readonly Coor<T>[] FourWayNeighbours =
    [
        new (-T.One, T.Zero),
        new (T.Zero, -T.One),
        new (T.Zero, T.One),
        new (T.One, T.Zero),
    ];

    public static readonly Coor<T>[] NineWayNeighbours =
    [
        new(-T.One, -T.One),
        new(-T.One, T.Zero),
        new(-T.One, T.One),
        new(T.Zero, -T.One),
        new(T.Zero, T.One),
        new(T.One, -T.One),
        new(T.One, T.Zero),
        new(T.One, T.One),
    ];

    public IEnumerable<Coor<T>> GetFourWayNeighbours()
        => FourWayNeighbours.Select(c => this + c);

    public bool IsOpposite(Coor<T> other) =>
        (this == Coor<T>.Right && other == Coor<T>.Left)
        || (this == Coor<T>.Left && other == Coor<T>.Right)
        || (this == Coor<T>.Up && other == Coor<T>.Down)
        || (this == Coor<T>.Down && other == Coor<T>.Up);

    public static bool operator ==(Coor<T> me, (T, T) other) => new Coor<T>(other.Item1, other.Item2) == me;
    public static bool operator !=(Coor<T> me, (T, T) other) => !(me == other);
    public static Coor<T> operator +(Coor<T> me, (T, T) other) => new(Y: me.Y + other.Item1, X: me.X + other.Item2);
    public static Coor<T> operator -(Coor<T> me, (T, T) other) => new(Y: me.Y - other.Item1, X: me.X - other.Item2);

    public static T ManhattanDistance(Coor<T> me, Coor<T> other)
    {
        var y = me.Y - other.Y;
        if (y < T.Zero)
        {
            y *= -T.One;
        }

        var x = me.X - other.X;
        if (x < T.Zero)
        {
            x *= -T.One;
        }

        return x + y;
    }

    public T ManhattanDistance(Coor<T> other) => ManhattanDistance(this, other);

    public override string ToString() => $"[{Y},{X}]";
}

public static class CoorExtensions
{
    public static bool InBoundsOf<R>(this Coor<int> coor, R[,] array)
        => coor.Y >= 0 && coor.Y < array.Height() && coor.X >= 0 && coor.X < array.Width();

    public static void Visualize(this ICollection<Coor<int>> coors, Coor<int>? min, Coor<int>? max)
    {
        min ??= new Coor<int>(coors.Min(c => c.Y), coors.Min(c => c.X));
        max ??= new Coor<int>(coors.Max(c => c.Y), coors.Max(c => c.X));

        var width = max.X - min.X + 1;
        var height = max.Y - min.Y + 1;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Console.Write(coors.Contains(new(y, x)) ? '#' : '.');
            }

            Console.WriteLine();
        }
    }
}


public static class MultiDimensionalArrayExtensions
{
    public static int Height<T>(this T[,] array) => array.GetLength(0);
    public static int Width<T>(this T[,] array) => array.GetLength(1);

    public static T[,] InitializeWith<T>(this T[,] array, T value)
    {
        var h = array.Height();
        var w = array.Width();
        for (int i = 0; i < h * w; i++)
        {
            array[i % h, i / h] = value;
        }

        return array;
    }

    public static T[,] InitializeWith<T>(this T[,] array, Func<int, int, T> filler)
    {
        var h = array.Height();
        var w = array.Width();
        for (int i = 0; i < h * w; i++)
        {
            var row = i % h;
            var col = i / h;
            array[row, col] = filler(row, col);
        }

        return array;
    }

    public static IEnumerable<Coor<int>> AllCoordinates<T>(this T[,] array)
    {
        for (int i = 0; i < array.Height() * array.Width(); i++)
        {
            yield return new Coor<int>(i % array.Height(), i / array.Height());
        }
    }

    public static void ForEach<T>(this T[,] array, Action<int, int, T> action)
    {
        for (int row = 0; row < array.Height(); row++)
            for (int col = 0; col < array.Width(); col++)
                action(row, col, array[row, col]);
    }

    public static string ToFlatString(this char[,] array)
    {
        var h = array.Height();
        var w = array.Width();
        var result = new StringBuilder(h * w);
        for (int i = 0; i < h * w; i++)
        {
            result.Append(array[i % h, i / h]);
        }

        return result.ToString();
    }

    public static void Print(this char[,] map, Func<Coor<int>, char?>? printOverride = null)
    {
        for (var y = 0; y < map.Height(); y++)
        {
            for (var x = 0; x < map.Width(); x++)
            {
                var c = printOverride?.Invoke(new(y, x)) ?? map[y, x];
                Console.Write(c);
            }

            Console.WriteLine();
        }
    }

    public static void Print<T>(this T[,] map, Func<Coor<int>, char> printOverride)
    {
        for (var y = 0; y < map.Height(); y++)
        {
            for (var x = 0; x < map.Width(); x++)
            {
                Console.Write(printOverride.Invoke(new(y, x)));
            }

            Console.WriteLine();
        }
    }

    public static T Get<T>(this T[,] items, Coor<int> coor)
        => items[coor.Y, coor.X];

    public static T Set<T>(this T[,] items, Coor<int> coor, T value)
        => items[coor.Y, coor.X] = value;
}

//using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
//using Beam = (System.Numerics.Complex pos, System.Numerics.Complex dir);
//using System.Linq;
//using System.Numerics;


//Console.WriteLine(new Solution().PartOne(File.ReadAllText("input.txt")));
//Console.WriteLine(new Solution().PartTwo(File.ReadAllText("input.txt")));

//class Solution 
//{

//    static readonly Complex Up = -Complex.ImaginaryOne;
//    static readonly Complex Down = Complex.ImaginaryOne;
//    static readonly Complex Left = -Complex.One;
//    static readonly Complex Right = Complex.One;

//    public object PartOne(string input) =>
//        EnergizedCells(ParseMap(input), (Complex.Zero, Right));

//    public object PartTwo(string input)
//    {
//        var map = ParseMap(input);
//        return (from beam in StartBeams(map) select EnergizedCells(map, beam)).Max();
//    }

//    // follow the beam in the map and return the energized cell count. 
//    int EnergizedCells(Map map, Beam beam)
//    {

//        // this is essentially just a flood fill algorithm.
//        var q = new Queue<Beam>([beam]);
//        var seen = new HashSet<Beam>();

//        while (q.TryDequeue(out beam))
//        {
//            seen.Add(beam);
//            foreach (var dir in Exits(map[beam.pos], beam.dir))
//            {
//                var pos = beam.pos + dir;
//                if (map.ContainsKey(pos) && !seen.Contains((pos, dir)))
//                {
//                    q.Enqueue((pos, dir));
//                }
//            }
//        }

//        return seen.Select(beam => beam.pos).Distinct().Count();
//    }

//    // go around the edges (top, right, bottom, left order) of the map
//    // and return the inward pointing directions
//    IEnumerable<Beam> StartBeams(Map map)
//    {
//        var br = map.Keys.MaxBy(pos => pos.Imaginary + pos.Real);
//        return [
//            ..from pos in map.Keys where pos.Real == 0 select (pos, Down),
//            ..from pos in map.Keys where pos.Real == br.Real select (pos, Left),
//            ..from pos in map.Keys where pos.Imaginary == br.Imaginary select (pos, Up),
//            ..from pos in map.Keys where pos.Imaginary == 0 select (pos, Right),
//        ];
//    }

//    // using a dictionary helps with bounds check (simply containskey):
//    Map ParseMap(string input)
//    {
//        var lines = input.Split(Environment.NewLine);
//        return (
//            from irow in Enumerable.Range(0, lines.Length)
//            from icol in Enumerable.Range(0, lines[0].Length)
//            let cell = lines[irow][icol]
//            let pos = new Complex(icol, irow)
//            select new KeyValuePair<Complex, char>(pos, cell)
//        ).ToDictionary();
//    }

//    // the 'exit' direction(s) of the given cell when entered by a beam moving in 'dir'
//    // we have some special cases for mirrors and spliters, the rest keeps the direction
//    Complex[] Exits(char cell, Complex dir) => cell switch
//    {
//        '-' when dir == Up || dir == Down => [Left, Right],
//        '|' when dir == Left || dir == Right => [Up, Down],
//        '/' => [-new Complex(dir.Imaginary, dir.Real)],
//        '\\' => [new Complex(dir.Imaginary, dir.Real)],
//        _ => [dir]
//    };
//}