using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Utils
{
    public static class Arrays
    {
        public static int NextIndex(int next, int length)
            => next < 0 ? length : (next >= length ? 0 : next);
    }
}
