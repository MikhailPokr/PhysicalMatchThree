using PMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;

internal class ActionBarController : IService
{
    private GemType[] _slots;
    private IActionBarRule _rule;

    public event Action<GemType[]> ActionBarChanged;
    public event Action ActionBarFull;

    public ActionBarController(int slotsCount, IActionBarRule rule)
    {
        EventBus<GemClickEvent>.Subscribe(OnClick);

        _slots = Enumerable.Repeat<GemType>(null, slotsCount).ToArray();
        _rule = rule;
    }

    public void ChangeActionBar(GemType[] slots)
    {
        _slots = slots;
        ActionBarChanged?.Invoke(_slots);
    }

    private void OnClick(GemClickEvent gemClickEvent)
    {
        if (_slots.All(x => x != null))
            return;
        int slotId = Array.IndexOf(_slots, _slots.First(x => x == null));
        _slots[slotId] = gemClickEvent.Gem.GemType;
        _slots[slotId].Effect?.ApplyInBar(this, _slots);

        _slots = _rule.Process(_slots);

        ActionBarChanged?.Invoke(_slots);
        if (_slots.All(x => x != null))
            ActionBarFull?.Invoke();
    }
}