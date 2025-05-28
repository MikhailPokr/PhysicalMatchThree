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
        public override RuneType Rune => RuneType.Isa;

        private int _unfrozenValue = 3;
        private int _lastValue;

        private IActionBarController _actionBarController;
        private Gem _gem;

        public override void ApplyInBar(GemType[] slots) { }

        public override void ApplyInField(Gem gem)
        {
            _gem = gem;
            _gem.SwitchLock();
            _actionBarController = ServiceLocator.Resolve<IActionBarController>();

            _actionBarController.ActionBarChanged += OnActionBarChanged;
        }

        public override void OnDestroy()
        {
            _actionBarController.ActionBarChanged -= OnActionBarChanged;
        }

        private void OnActionBarChanged(GemType[] slots)
        {
            int count = slots.Count(x => x != null);
            if (count < _lastValue)
                _unfrozenValue--;
            _lastValue = count;

            if (_unfrozenValue == 0)
            {
                _gem.View.Rune.color = Color.black;
                _actionBarController.ActionBarChanged -= OnActionBarChanged;
                _gem.SwitchLock();
            }
        }
    }
}
