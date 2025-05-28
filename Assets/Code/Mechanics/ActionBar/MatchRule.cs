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
            if (slots.Length == 0 || _count <= 1)
                return slots;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    continue;

                GemType currentType = slots[i];
                int foundCount = 1; 

                for (int j = i + 1; j < slots.Length; j++)
                {
                    if (slots[j] != null && slots[j] == currentType)
                        foundCount++;
                }

                if (foundCount >= _count)
                {
                    GemType[] newSlots = new GemType[slots.Length];
                    int newIndex = 0;

                    foreach (GemType gem in slots)
                    {
                        if (gem == null || gem != currentType)
                        {
                            newSlots[newIndex] = gem;
                            newIndex++;
                        }
                    }

                    return newSlots;
                }
            }

            return slots;
        }
    }
}
