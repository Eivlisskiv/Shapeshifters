using System;
using System.Collections.Generic;

namespace IgnitedBox.Random.DropTables
{
    public class ExpTable<T> : DropTable<T>
    {
        //https://www.desmos.com/calculator/co810wj86t

        const int width = 100;

        public double RateDistribution
        {
            get => _curve;
            set
            {
                _curve =  value;
            }
        }

        private double _curve = 1.01;

        public ExpTable(double rateDistribution) : base()
        {
            RateDistribution = rateDistribution;
        }

        public ExpTable(double rateDistribution, IEnumerable<T> items) : base(items)
        {
            RateDistribution = rateDistribution;
        }

        public ExpTable(double rateDistribution, params T[] items) : base(items)
        {
            RateDistribution = rateDistribution;
        }

        private protected override int DropIndex()
        {
            if (Count == 0) return -1;
            if (_curve <= 1) return RandomInt(Count);

            double x = RandomDouble(width);

            double a = Count / (Math.Pow(_curve, width) - 1);
            double y = a * Math.Pow(_curve, x) - a;
            return (int)y;
        }

        public double[] GetRates()
        {
            double[] rates = new double[Count];
            double prev = 0;
            for(int i = 0; i < rates.Length; i++)
            {
                prev = GetRate(i + 1) - prev;
                rates[i] = prev;
            }
            return rates;
        }

        private double GetRate(int y)
        {
            if (_curve <= 1) return 1.00 / Count;

            double a = y * (Math.Pow(_curve, width) - 1);
            a = Math.Log((a + Count) / Count);
            return (a / Math.Log(_curve));
        }
    }
}
