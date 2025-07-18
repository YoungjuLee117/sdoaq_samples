using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDOAQNet.Tool
{
    /// <summary>
    /// Focus List Low, High, Uint
    /// </summary>
    public class FocusLHU
    {
        public int Low { get; private set; }
        public int High { get; private set; }
        public int Unit { get; private set; }
        
        public void SetFocusList(int low, int high, int unit)
        {
            Low = low;
            High = high;
            Unit = unit;
        }

        public int[] GetStepList()
        {
            var stepList = new List<int>();

            for (int step = Low; step <= High; step += Unit)
            {
                stepList.Add(step);
            }

            return stepList.ToArray();
        }

        public new string ToString()
        {
            return $"{Low}-{High}-{Unit}";
        }
    }
}
