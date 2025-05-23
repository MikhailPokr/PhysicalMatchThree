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

            int start = 0;
            while (start < result.Length)
            {
                if (result[start] == null)
                {
                    start++;
                    continue;
                }

                int end = start;
                while (end < result.Length &&
                       result[end] != null &&
                       result[end].IsSameType(result[start]))
                {
                    end++;
                }

                int count = end - start;
                if (count >= _count)
                {
                    for (int i = start; i < end; i++)
                    {
                        result[i] = null;
                    }
                }

                start = end;
            }

            return result;
        }
    }
}
