using System;
using System.Collections.Generic;

namespace IgnitedBox.Random.DropTables
{
    public class ExpTable<T> : DropTable<T>
    {
        //https://www.desmos.com/calculator/co810wj86t

        const int width = 100;

        public double VerticalDistribution
        {
            get => vCurve;
            set
            {
                vCurve =  Math.Max(1, value);
            }
        }

        private double vCurve = 1.01;

        public double HorizontalDistribution
        {
            get => hCurve;
            set
            {
                hCurve = Math.Max(1, value);
            }
        }

        private double hCurve = 1.01;

        public ExpTable(double verticalDistribution, int horizontalDistribution) : base()
        {
            VerticalDistribution = verticalDistribution;
            HorizontalDistribution = horizontalDistribution;
        }

        public ExpTable(double verticalDistribution, int horizontalDistribution, IEnumerable<T> items) : base(items)
        {
            VerticalDistribution = verticalDistribution;
            HorizontalDistribution = horizontalDistribution;
        }

        public ExpTable(double verticalDistribution, int horizontalDistribution, params T[] items) : base(items)
        {
            VerticalDistribution = verticalDistribution;
            HorizontalDistribution = horizontalDistribution;
        }

        private protected override int DropIndex()
        {
            if (Count == 0) return -1;
            if (hCurve <= 1) return RandomInt(Count);

            double x = RandomDouble(width);

            double a = Count / (Math.Pow(hCurve, width) - 1);
            double y = a * Math.Pow(hCurve, x) - a;
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
            if (hCurve <= 1) return 1.00 / Count;

            double a = y * (Math.Pow(hCurve, width) - 1);
            a = Math.Log((a + Count) / Count);
            return (a / Math.Log(hCurve));
        }
    }
}
