using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PMT
{
    internal class MatchRule : IActionBarRule
    {
        private int _count;
        public MatchRule(int count)
        {
            _count = count;
        }

        public GemType[] Process(GemType[] slots)
        {
            if (slots.Length == 0) return slots;

            GemType[] result = (GemType[])slots.Clone();
            bool[] toRemove = new bool[result.Length];

            var groups = result
                .Where(g => !g.IsNull)
                .GroupBy(g => g)
                .Where(g => g.Count() >= _count);

            foreach (var group in groups)
            {
                GemType typeToRemove = group.Key;
                for (int i = 0; i < result.Length; i++)
                {
                    if (!result[i].IsNull && result[i] == typeToRemove)
                    {
                        toRemove[i] = true;
                    }
                }
            }

            int writeIndex = 0;
            for (int readIndex = 0; readIndex < result.Length; readIndex++)
            {
                if (!toRemove[readIndex])
                {
                    result[writeIndex] = result[readIndex];
                    writeIndex++;
                }
            }

            for (int i = writeIndex; i < result.Length; i++)
            {
                result[i] = GemType.Default;
            }

            return result;
        }
    }
}
