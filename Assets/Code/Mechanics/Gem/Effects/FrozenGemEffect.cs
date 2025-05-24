using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PMT
{
    internal class FrozenGemEffect : BaseGemSpecialEffect
    {
        public override Color Color => new Color(0, 0, 0.8f);

        private int _unfrozenValue = 3;
        private int _lastValue;

        private ActionBarController _actionBarController;
        private Gem _gem;

        public override void ApplyInBar(ActionBarController actionBarController, GemType[] slots) { }

        public override void ApplyInField(Gem gem)
        {
            _gem = gem;
            _gem.SwitchLock();
            _actionBarController = ServiceLocator.Resolve<ActionBarController>();

            _actionBarController.ActionBarChanged += OnActionBarChanged;
        }

        private void OnActionBarChanged(GemType[] slots)
        {
            int count = slots.Count(x => x != null);
            if (count < _lastValue)
                _unfrozenValue--;
            _lastValue = count;

            if (_unfrozenValue == 0)
            {
                _gem.View.Outline.color = Color.black;
                _actionBarController.ActionBarChanged -= OnActionBarChanged;
                _gem.SwitchLock();
            }
        }
    }
}
