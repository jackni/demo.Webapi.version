using System;
using System.Linq;

namespace Demo.Domain
{
    public class SimpleMath : ISimpleMath, ISimpleMathV2
    {
        public float Add(float x, float y)
        {
            return x + y;
        }

        public float Sum(float[] array)
        {
            return array.Sum();
        }
    }
}
