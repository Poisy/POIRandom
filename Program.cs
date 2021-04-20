using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace POIRandom
{
    class Program
    {
        static void Main(string[] args)
        {
            string result = GetResult(args);
            Console.WriteLine(result);
        }

        static string GetResult(string[] args)
        {
            RandomGenerator random = new RandomGenerator();
            StringBuilder stringBuilder = new StringBuilder();
            int repeats = 1;

            if (args.Length == 0) return PrintInfo();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--repeat" && args.Length > ++i)
                {
                    Int32.TryParse(args[i], out repeats);
                }
            }

            args = args[0].Split('-');

            if (TryGetRandomInt(stringBuilder, random, repeats, args)) { }
            else if (TryGetRandomDouble(stringBuilder, random, repeats, args)) { }
            else GetRandomString(stringBuilder, random, repeats, args);

            return stringBuilder.Remove(0, 1).ToString();
        }

        static void GetRandom(StringBuilder stringBuilder, int repeats, Func<string> randomFunction)
        {
            while (repeats != 0)
            {
                stringBuilder.Append("\n=> " + randomFunction.Invoke());
                repeats--;
            }
        }

        static bool TryGetRandomInt(StringBuilder stringBuilder, RandomGenerator random, int repeat, string[] args)
        {
            bool state = false;
            
            if (args.Length == 2)
            {
                int i1, i2;
                if (Int32.TryParse(args[0], out i1) && Int32.TryParse(args[1], out i2))
                {
                    GetRandom(stringBuilder, repeat, () => random.RandomInt(i1, i2).ToString());
                    
                    state = true;
                } 
            }
            
            return state;
        }

        static bool TryGetRandomDouble(StringBuilder stringBuilder, RandomGenerator random, int repeat, string[] args)
        {
            bool state = false;
            
            if (args.Length == 2 && (args[0].Contains('.') && args[1].Contains('.')))
            {
                double d1, d2;
                if (Double.TryParse(args[0], out d1) && Double.TryParse(args[1], out d2))
                {
                    GetRandom(stringBuilder, repeat, () => random.RandomDouble(d1, d2).ToString());
                    
                    state = true;
                }   
            }
            
            return state;
        }

        static void GetRandomString(StringBuilder stringBuilder, RandomGenerator random, int repeat, string[] args)
        {
            if (args.All(a => a.Contains(':')))
            {
                Dictionary<string, int> choices = new Dictionary<string, int>();

                for (int i = 0; i < args.Length; i++)
                {
                    var temp = args[i].Split(':');
                    
                    if (temp.Length == 2)
                    {
                        string choice = temp[0];
                        int percent = 0;

                        Int32.TryParse(temp[1], out percent);

                        choices.Add(choice, percent);
                    }
                }

                GetRandom(stringBuilder, repeat, () => random.RandomStringWithChance(choices));
            }
            else GetRandom(stringBuilder, repeat, () => random.RandomString(args));
        }

        static string PrintInfo()
        {
            return "The program takes one argument.\n" + 
            "Random number between 1 and 5 => \"1-5\".\n" + 
            "Random floating point number between 1.52 and 4.32 => \"1.52-4.32\".\n" +
            "Random choice between apple, orange and banana => \"apple-orange-banana\".\n";
        }
    }
}
