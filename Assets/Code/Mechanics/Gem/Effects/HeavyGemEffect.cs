using System;
using UnityEngine;

namespace PMT
{
    internal class HeavyGemEffect : BaseGemSpecialEffect
    {
        public override RuneType Rune => RuneType.Hagalaz;

        public override void ApplyInBar(GemType[] slots) { }

        public override void ApplyInField(Gem gem)
        {
            gem.GetComponent<Rigidbody2D>().mass = 30;
        }

        public override void OnDestroy() { }
    }
}
