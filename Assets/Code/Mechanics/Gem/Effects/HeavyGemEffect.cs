using System;
using UnityEngine;

namespace PMT
{
    internal class HeavyGemEffect : BaseGemSpecialEffect
    {
        public override Color Color => Color.gray;

        public override void ApplyInBar(ActionBarController actionBarController, GemType[] slots) { }

        public override void ApplyInField(Gem gem)
        {
            gem.GetComponent<Rigidbody2D>().mass = 30;
        }
    }
}
