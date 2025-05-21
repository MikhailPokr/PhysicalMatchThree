using System;
using System.Collections.Generic;
using UnityEngine;

public class GemView : MonoBehaviour
{
    private List<GemView> _gemsNear = new();

    public event Action<List<GemView>> MatchThree;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GemView other = collision.gameObject.GetComponent<GemView>();
        if (other != null)
        {
            print(other.name + "Enter");
            _gemsNear.Add(other);
        }

        if (_gemsNear.Count > 1)
        {
            List<GemView> gems = new();
            gems.AddRange(_gemsNear);
            gems.Add(this);
            MatchThree?.Invoke(gems);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GemView other = collision.gameObject.GetComponent<GemView>();
        if (other != null)
        {
            print(other.name + "Exit");
            _gemsNear.Remove(other);
        }
    }
}
