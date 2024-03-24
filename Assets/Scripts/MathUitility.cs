using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MathUitility
{
    public static float LoopClamp(float value, float minInclusive, float maxExclusive)
    {
        if (value >= maxExclusive)
            value = minInclusive;
        else if (value < minInclusive)
            value = maxExclusive - 1;

        return value;
    }
    
    public static int LoopClamp(int value, int minInclusive, int maxExclusive)
    {
        if (value >= maxExclusive)
            value = minInclusive;
        else if (value < minInclusive)
            value = maxExclusive - 1;

        return value;
    }
}
