using System;
using UnityEngine;

namespace PMT
{
    internal class HeavyGemEffect : BaseGemSpecialEffect
    {
        public override Color Color => Color.gray;

        public override void ApplyInBar(GemType[] slots) { }

        public override void ApplyInField(Gem gem)
        {
            gem.GetComponent<Rigidbody2D>().mass = 30;
        }

        public override void OnDestroy() { }
    }
}
