using UnityEngine;

namespace PMT
{
    internal abstract class BaseGemSpecialEffect
    {
        public abstract Color Color { get; }

        public abstract void ApplyInField(Gem gem);

        public abstract void ApplyInBar(GemType[] slots);

        public abstract void OnDestroy();
    }
}
