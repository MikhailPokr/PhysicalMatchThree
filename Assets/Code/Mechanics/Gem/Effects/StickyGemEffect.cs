using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PMT
{
    internal class StickyGemEffect : BaseGemSpecialEffect
    {
        public override Color Color => new Color(0, 0.8f, 0);

        public override void ApplyInBar(GemType[] slots) { }

        private Gem _gem;

        public override void ApplyInField(Gem gem)
        {
            _gem = gem;
            gem.Match += OnMatch;
        }

        private void OnMatch(Gem other)
        {
            FixedJoint2D joint = other.gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = _gem.gameObject.GetComponent<Rigidbody2D>();
        }

        public override void OnDestroy()
        {
            _gem.Match -= OnMatch;
        }
    }
}
