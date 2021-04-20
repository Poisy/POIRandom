using System;
using System.Collections.Generic;
using System.Linq;

namespace POIRandom
{
    public class RandomGenerator
    {
        private readonly Random _random;

        public RandomGenerator()
        {
            _random = new Random();
        }

        public int RandomInt(int from, int to)
        {
            (from, to) = (from > to ? to : from, from > to ? from : to);

            int result = _random.Next(from, to+1);
            return result;
        }

        public double RandomDouble(double from, double to, int place = 2)
        {
            double result = _random.NextDouble() * (to - from) + from;
            result = Math.Round(result, place);
            return result;
        }

        public string RandomString(string[] array)
        {
            if (array.Length == 0) return null;

            string result = array[RandomInt(0, array.Length-1)];
            return result;
        }

        public string RandomStringWithChance(Dictionary<string, int> choices)
        {
            string[] keys = choices.Keys.ToArray();
            int[] values = choices.Values.ToArray();

            // Checking if the sum of percentages are 100%
            if (values.Sum() != 100) return null;

            int randInt = RandomInt(1, 101);
            string result = "";

            for (int i = 1; i < values.Length; i++) values[i] = values[i] + values[i-1];

            for (int i = 0; i < values.Length; i++)
            {
                if (randInt <= values[i])
                {
                    result = keys[i];
                    break;
                }
            }

            return result;
        }
    }
}