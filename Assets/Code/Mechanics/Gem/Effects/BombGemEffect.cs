using System;
using System.Linq;
using UnityEngine;

namespace PMT
{
    internal class BombGemEffect : BaseGemSpecialEffect
    {
        public override RuneType Rune => RuneType.Thuriasaz;

        private IActionBarController _actionBarController;
        private GemType[] _victims;
        private bool _isProcessing; 

        public override void ApplyInBar(GemType[] slots)
        {
            _victims = new GemType[2];
            _actionBarController = ServiceLocator.Resolve<IActionBarController>();
            _actionBarController.ActionBarChanged += OnActionBarChanged;

            UpdateVictims(slots);
        }

        private void UpdateVictims(GemType[] slots)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].Effect == this)
                {
                    int pre = i - 1;
                    int after = i + 1;

                    if (pre > -1)
                        _victims[0] = slots[pre];
                    if (after < slots.Length)
                        _victims[1] = slots[after];
                    return;
                }
            }

            _victims[0] = null;
            _victims[1] = null;
        }

        private void OnActionBarChanged(GemType[] slots)
        {
            if (_isProcessing) return;
            _isProcessing = true;

            bool bombExists = slots.Any(gem => gem != null && gem.Effect == this);

            if (!bombExists && (_victims[0] != null || _victims[1] != null))
            {
                for (int i = 0; i < _victims.Length; i++)
                {
                    if (_victims[i] != null && slots.Contains(_victims[i]))
                    {
                        int index = Array.IndexOf(slots, _victims[i]);
                        slots[index] = null;
                    }
                }
                _victims[0] = null;
                _victims[1] = null;

                _actionBarController.ChangeActionBar(slots);
            }
            else
            {
                UpdateVictims(slots);
            }

            _isProcessing = false;
        }

        public override void OnDestroy()
        {
            if (_actionBarController == null)
                return;
            _actionBarController.ActionBarChanged -= OnActionBarChanged;
        }

        public override void ApplyInField(Gem gem) { }
    }
}