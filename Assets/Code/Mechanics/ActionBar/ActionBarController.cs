using PMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ActionBarController : IService
{
    private GemType[] _slots;
    private IActionBarRule _rule;

    public event Action<GemType[]> ActionBarChanged;

    public ActionBarController(int slotsCount, IActionBarRule rule)
    {
        EventBus<GemClickEvent>.Subscribe(OnClick);

        _slots = Enumerable.Repeat<GemType>(null, slotsCount).ToArray();
        _rule = rule;
    }

    private void OnClick(GemClickEvent gemClickEvent)
    {
        int slotId = Array.IndexOf(_slots, null);
        _slots[slotId] = gemClickEvent.Gem.GemType;
        _slots = _rule.Process(_slots);

        ActionBarChanged?.Invoke(_slots);
    }
}