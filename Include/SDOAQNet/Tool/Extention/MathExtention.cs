using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDOAQNet.Tool
{
    public static class MathExtention
    {
        public static bool Equal(this double a, double b, double epsilon = 1e-10)
        {
            return Math.Abs(a - b) < epsilon;
        }

        public static bool Equal(this float a, float b, double epsilon = 1e-6f)
        {
            return Math.Abs(a - b) < epsilon;
        }
    }
}
