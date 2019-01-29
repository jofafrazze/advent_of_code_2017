using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day21
{
    class SqBitmap : IEquatable<SqBitmap>
    {
        private bool hashOk;
        private int hash;
        public int Side { get; }
        private char[,] data;
        public char[,] Data { get { hashOk = false; return data; } }
        public char this[int x, int y]
        {
            get
            {
                return data[x, y];
            }
        }
        public SqBitmap(int s)
        {
            hashOk = false;
            Side = s;
            data = new char[s,s];
        }
        public SqBitmap(SqBitmap bmp) : this(bmp.Side)
        {
            for (int y = 0; y < Side; y++)
                for (int x = 0; x < Side; x++)
                    data[x, y] = bmp[x, y];
        }
        public SqBitmap GetSubSquare(int xOffs, int yOffs, int size)
        {
            SqBitmap bmp = new SqBitmap(size);
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    bmp.Data[x, y] = data[xOffs + x, yOffs + y];
            return bmp;
        }
        public void SetSubSquare(int xOffs, int yOffs, SqBitmap bmp)
        {
            hashOk = false;
            for (int y = 0; y < bmp.Side; y++)
                for (int x = 0; x < bmp.Side; x++)
                    data[xOffs + x, yOffs + y] = bmp.data[x, y];
        }

        // -----
        public override bool Equals(Object obj)
        {
            return obj is SqBitmap && Equals((SqBitmap)obj);
        }
        public bool Equals(SqBitmap other)
        {
            return other != null &&
                   Side == other.Side &&
                   data.Cast<char>().SequenceEqual(other.data.Cast<char>());
            //       EqualityComparer<char[,]>.Default.Equals(data, other.data);
        }
        public override int GetHashCode()
        {
            if (!hashOk)
            {
                hash = Side.GetHashCode();
                if (data != null)
                {
                    foreach (char c in data.Cast<char>())
                    {
                        hash *= 17;
                        hash += c.GetHashCode();
                    }
                }
                hashOk = true;
            }
            return hash;
        }
        //public override int GetHashCode()
        //{
        //    return HashCode.Combine(side, data);
        //}
        public static bool operator ==(SqBitmap bitmap1, SqBitmap bitmap2)
        {
            return EqualityComparer<SqBitmap>.Default.Equals(bitmap1, bitmap2);
        }
        public static bool operator !=(SqBitmap bitmap1, SqBitmap bitmap2)
        {
            return !(bitmap1 == bitmap2);
        }

        // -----
        public SqBitmap FlipVertical()
        {
            SqBitmap a = new SqBitmap(Side);
            for (int y = 0; y < Side; y++)
                for (int x = 0; x < Side; x++)
                    a.Data[x, y] = data[x, Side - 1 - y];
            return a;
        }
        public SqBitmap FlipHorizontal()
        {
            SqBitmap a = new SqBitmap(Side);
            for (int y = 0; y < Side; y++)
                for (int x = 0; x < Side; x++)
                    a.Data[x, y] = data[Side - 1 - x, y];
            return a;
        }
        public SqBitmap Rotate90CW()
        {
            SqBitmap a = new SqBitmap(Side);
            for (int y = 0; y < Side; y++)
                for (int x = 0; x < Side; x++)
                    a.Data[x, y] = data[y, Side - 1 - x];
            return a;
        }
    }

    class Day21
    {
        static SqBitmap BmpFromStr(string s)
        {
            SqBitmap bmp = new SqBitmap(s.IndexOf('/'));
            for (int y = 0; y < bmp.Side; y++)
                for (int x = 0; x < bmp.Side; x++)
                    bmp.Data[x, y] = s[(y * (bmp.Side + 1)) + x];
            return bmp;
        }

        static Dictionary<SqBitmap, SqBitmap> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            Dictionary<SqBitmap, SqBitmap> dict = new Dictionary<SqBitmap, SqBitmap>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] s = line.Split(" => ").ToArray();
                SqBitmap a = BmpFromStr(s[0]);
                SqBitmap b = BmpFromStr(s[1]);
                dict[a] = b;
            }
            return dict;
        }

        static HashSet<SqBitmap> GetAllPermutations(SqBitmap arg)
        {
            HashSet<SqBitmap> set = new HashSet<SqBitmap>();
            SqBitmap bmp = new SqBitmap(arg);
            for (int r = 0; r < 2; r++)
            {
                set.Add(bmp);
                set.Add(bmp.FlipVertical());
                set.Add(bmp.FlipHorizontal());
                set.Add(bmp.FlipVertical().FlipHorizontal());
                bmp = bmp.Rotate90CW();
            }
            return set;
        }

        static SqBitmap GetSubSubstitution(HashSet<SqBitmap> permutations, Dictionary<SqBitmap, SqBitmap> subst)
        {
            foreach (SqBitmap p in permutations)
            {
                if (subst.ContainsKey(p))
                    return subst[p];
            }
            throw new ArgumentOutOfRangeException();
        }

        static SqBitmap Transform(SqBitmap bmp, Dictionary<SqBitmap, SqBitmap> subst)
        {
            int subSide = (bmp.Side % 2 == 0) ? 2 : 3;
            int n = bmp.Side / subSide;
            SqBitmap next = new SqBitmap(n * (subSide + 1));
            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    SqBitmap sub = bmp.GetSubSquare(x * subSide, y * subSide, subSide);
                    HashSet<SqBitmap> permutations = GetAllPermutations(sub);
                    foreach (SqBitmap p in permutations)
                    {
                        if (sub == p)
                        {
                            SqBitmap newSub = GetSubSubstitution(permutations, subst);
                            next.SetSubSquare(x * (subSide + 1), y * (subSide + 1), newSub);
                            break;
                        }
                    }
                }
            }
            return next;
        }

        static void PartAB()
        {
            Dictionary<SqBitmap, SqBitmap> subst = ReadInput();
            SqBitmap bmp = BmpFromStr(".#./..#/###");
            int iter = 0;
            const int iterA = 5;
            const int iterB = 18;
            for (; iter < iterA; iter++)
                bmp = Transform(bmp, subst);
            Console.WriteLine("Part A: Result is {0}.", bmp.Data.Cast<char>().Where(a => a == '#').Count());
            for (; iter < iterB; iter++)
            {
                Console.WriteLine("  iteration {0}/{1}", iter + 1, iterB);
                bmp = Transform(bmp, subst);
            }
            Console.WriteLine("Part B: Result is {0} (Bitmap side is {1}).", bmp.Data.Cast<char>().Where(a => a == '#').Count(), bmp.Side);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day21).Namespace + ":");
            PartAB();
        }
    }
}
