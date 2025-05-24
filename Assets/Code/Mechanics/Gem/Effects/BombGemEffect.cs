using System;
using System.Linq;
using UnityEngine;

namespace PMT
{
    internal class BombGemEffect : BaseGemSpecialEffect
    {
        public override Color Color => new Color(0.8f, 0, 0);

        private ActionBarController _actionBarController;
        private GemType[] _victims;
        private bool _isProcessing; // Флаг для предотвращения рекурсии

        public override void ApplyInBar(ActionBarController actionBarController, GemType[] slots)
        {
            _victims = new GemType[2];
            _actionBarController = actionBarController;
            _actionBarController.ActionBarChanged += OnActionBarChanged;

            // Первоначальное сохранение соседей
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
            // Если бомба исчезла, очищаем жертв
            _victims[0] = null;
            _victims[1] = null;
        }

        private void OnActionBarChanged(GemType[] slots)
        {
            if (_isProcessing) return; // Защита от рекурсии
            _isProcessing = true;

            // Проверяем, есть ли ещё бомба в слотах
            bool bombExists = slots.Any(gem => gem != null && gem.Effect == this);

            if (!bombExists && (_victims[0] != null || _victims[1] != null))
            {
                // Если бомбы нет, но есть жертвы — удаляем их
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

                // Уведомляем об изменениях
                _actionBarController.ChangeActionBar(slots);
            }
            else
            {
                // Обновляем список жертв, если бомба ещё есть
                UpdateVictims(slots);
            }

            _isProcessing = false;
        }

        public override void ApplyInField(Gem gem) { }
    }
}