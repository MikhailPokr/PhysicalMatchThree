using PMT;
using System;

internal interface IActionBarController : IService
{
    event Action<GemType[]> ActionBarChanged;
    event Action ActionBarFull;

    void ChangeActionBar(GemType[] slots);
}