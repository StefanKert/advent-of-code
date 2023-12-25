using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.RegularExpressions;
using Hailstones = System.Collections.Generic.List<Hailstone>;

Console.WriteLine("Part 1:" + new Solution().PartOne(File.ReadAllLines("input.txt")));
Console.WriteLine("Part 2:" + new Solution().PartTwo(File.ReadAllLines("input.txt")));

var hailStones = ParseHailstones(File.ReadAllLines("demo.txt"));
for (int i = 0; i < hailStones.Count; i++)
{
    var hailStonesA = hailStones[i];
    for (int j = i + 1; j < hailStones.Count; j++)
    {
        DoesIntersect(hailStones[i], hailStones[j], 7, 27);
        Console.WriteLine();
    }
}

static bool DoesIntersect(Hailstone hailstone1, Hailstone hailstone2, double minValue, double maxValue)
{
    var direction1 = hailstone1.py > 0;
    var direction2 = hailstone2.py > 0;

    var m1 = hailstone1.vy / hailstone1.vx;
    var b1 = hailstone1.py - m1 * hailstone1.px;

    var m2 = hailstone2.vy / hailstone2.vx;
    var b2 = hailstone2.py - m2 * hailstone2.px;

    // m1 * x + b1 = m2 * x + b2
    // (m1 - m2) * x = b2 - b1
    // x = (b2 - b1) / (m1 - m2)
    var x = (b2 - b1) / (m1 - m2);
    var y = m1 * x + b1;

    Console.WriteLine($"Hailstone A: {hailstone1.px} {hailstone1.py} {hailstone1.pz} @ {hailstone1.vx} {hailstone1.vy} {hailstone1.vz}");
    Console.WriteLine($"Hailstone B: {hailstone2.px} {hailstone2.py} {hailstone2.pz} @ {hailstone2.vx} {hailstone2.vy} {hailstone2.vz}");
    var intersect = false;

    if (x == double.PositiveInfinity || y == double.PositiveInfinity || x == double.NegativeInfinity || y == double.NegativeInfinity)
    {
        Console.WriteLine($"Hailstones' paths are parallel; they never intersect.");
        return false;
    }
    if (x < minValue && y < minValue && x > maxValue && y > maxValue)
    {
        Console.WriteLine($"Hailstones' paths will cross outside the test area (at x={x}, y={y}).");
        return false;
    }

    var isPassed1 = hailstone1.py > 0 ? hailstone1.py > y : hailstone1.py < y;
    var isPassed2 = hailstone2.py < 0 ? hailstone2.py > y : hailstone2.py < y;


    if (isPassed1 && isPassed2)
    {
        Console.WriteLine($"Hailstones' paths crossed in the past for both hailstones.");
        return false;
    }
    if (isPassed1 )
    {
        Console.WriteLine($"Hailstones' paths crossed in the past for hailstone A.");
        return false;
    }

    if (isPassed2)
    {
        Console.WriteLine($"Hailstones' paths crossed in the past for hailstone B.");
        return false;
    }

    Console.WriteLine($"Hailstones' paths will cross inside the test area (at x={x}, y={y}).");
    return true;
}

static void GetY(Hailstone hailstone)
{

}


static Hailstones ParseHailstones(string[] input) => (
    from line in input
    let parts = line.Split('@')
    let positions = parts[0].Split(",")
    let velocities = parts[1].Split(",")
    select new Hailstone(double.Parse(positions[0]), double.Parse(positions[1]), double.Parse(positions[2]), double.Parse(velocities[0]), double.Parse(velocities[1]), double.Parse(velocities[2]))
).ToList();

record Hailstone(double px, double py, double pz, double vx, double vy, double vz);

record Range(decimal begin, decimal end);
record Particle(Vec2 pos, Vec2 vel);
record Particle3(Vec3 pos, Vec3 vel);

class Solution 
{

    public object PartOne(string[] input)
    {
        var particles = ParseParticles(input);
        var areaBegin = 200000000000000;
        var areaEnd = 400000000000000;
        var res = 0;
        for (var i = 0; i < particles.Length; i++)
        {
            for (var j = i + 1; j < particles.Length; j++)
            {
                var mp = MeetPoint(particles[i], particles[j]);
                if (mp == null)
                {
                    continue;
                }
                if (areaBegin > mp.x || mp.x > areaEnd)
                {
                    continue;
                }

                if (areaBegin > mp.y || mp.y > areaEnd)
                {
                    continue;
                }
                if (Past(particles[i], mp))
                {
                    continue;
                }
                if (Past(particles[j], mp))
                {
                    continue;
                }
                res++;
            }
        }
        return res;
    }

    public object PartTwo(string[] input)
    {
        var particles = ParseParticles3(input);
        return Solve(v => v.x, particles) + Solve(v => v.y, particles) + Solve(v => v.z, particles);
    }

    bool Past(Particle p, Vec2 v)
    {
        // p.pos.x + t * p.vel.x = v.x
        if (p.vel.x == 0)
        {
            return true;
        }
        return (v.x - p.pos.x) / p.vel.x < 0;
    }

    Vec2 MeetPoint(Particle p1, Particle p2)
    {
        Mat2 m1 = new Mat2(
            p1.vel.y, -p1.vel.x,
            p2.vel.y, -p2.vel.x
        );

        var det = m1.Det;
        if (det == 0)
        {
            return null;
        }

        Vec2 v = new Vec2(
            p1.vel.y * p1.pos.x - p1.vel.x * p1.pos.y,
            p2.vel.y * p2.pos.x - p2.vel.x * p2.pos.y
        );

        return m1.Inv() * v;
    }

    BigInteger Solve(Func<Vec3, BigInteger> dim, Particle3[] particles)
    {
        for (var v0 = -10000; v0 < 10000; v0++)
        {
            var items = new List<(BigInteger dv, BigInteger x)>();
            foreach (var p in particles)
            {
                var dv = v0 - dim(p.vel);
                if (IsPrime(dv) && items.All(i => i.dv != dv))
                {
                    items.Add((dv: dv, x: dim(p.pos)));
                }
            }
            if (items.Count > 1)
            {
                var p0 = ChineseRemainderTheorem(items.ToArray());
                var ok = true;
                foreach (var p in particles)
                {
                    var dv = v0 - dim(p.vel);
                    var dx = dim(p.pos) > p0 ? dim(p.pos) - p0 : p0 - dim(p.pos);
                    if (dv == 0)
                    {
                        if (dx != 0)
                        {
                            ok = false;
                        }
                    }
                    else
                    {
                        if (dx % dv != 0)
                        {
                            ok = false;
                        }
                    }
                }
                if (ok)
                {
                    return p0;
                }
            }
        }
        throw new Exception();

    }

    public static bool IsPrime(BigInteger number)
    {
        if (number <= 2) return false;
        if (number % 2 == 0) return false;

        for (int i = 3; i * i <= number; i += 2)
            if (number % i == 0)
                return false;

        return true;
    }

    Particle[] ParseParticles(string[] input) => (
        from line in input
        let v = Regex.Matches(line, @"-?\d+").Select(m => decimal.Parse(m.Value)).ToArray()
        select new Particle(new Vec2(v[0], v[1]), new Vec2(v[3], v[4]))
    ).ToArray();


    Particle3[] ParseParticles3(string[] input) => (
        from line in input
        let v = Regex.Matches(line, @"-?\d+").Select(m => BigInteger.Parse(m.Value)).ToArray()
        select new Particle3(new Vec3(v[0], v[1], v[2]), new Vec3(v[3], v[4], v[5]))
    ).ToArray();

    // https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
    BigInteger ChineseRemainderTheorem((BigInteger mod, BigInteger a)[] items)
    {
        var prod = items.Aggregate(BigInteger.One, (acc, item) => acc * item.mod);
        var sum = items.Select((item, i) => {
            var p = prod / item.mod;
            return item.a * ModInv(p, item.mod) * p;
        });

        var s = BigInteger.Zero;
        foreach (var item in sum)
        {
            s += item;
        }

        return s % prod;
    }

    BigInteger ModInv(BigInteger a, BigInteger m) => BigInteger.ModPow(a, m - 2, m);

}

record Vec2(decimal x, decimal y) { }

record Vec3(BigInteger x, BigInteger y, BigInteger z)
{
    public static Vec3 operator +(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }
    public static Vec3 operator -(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }
    public static Vec3 operator *(BigInteger d, Vec3 v1)
    {
        return new Vec3(d * v1.x, d * v1.y, d * v1.z);
    }
}

record Mat2(decimal a, decimal b, decimal c, decimal d)
{
    public decimal Det => a * d - b * c;
    public Mat2 Inv()
    {
        var det = Det;
        return new Mat2(d / det, -b / det, -c / det, a / det);
    }

    public static Mat2 operator *(Mat2 m1, Mat2 m2)
    {
        return new Mat2(
            m1.a * m2.a + m1.b * m2.c,
            m1.a * m2.b + m1.b * m2.d,
            m1.b * m2.a + m1.d * m2.c,
            m1.b * m2.b + m1.d * m2.d
        );
    }

    public static Vec2 operator *(Mat2 m1, Vec2 v)
    {
        return new Vec2(
            m1.a * v.x + m1.b * v.y,
            m1.c * v.x + m1.d * v.y
        );
    }
}