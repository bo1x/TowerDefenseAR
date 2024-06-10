using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocidad : Switcher
{
    private void Start()
    {
        if (GameManager.Instance.IsTimeScaled())
            ButtonOn();
        else
            ButtonOff();
    }

    public void OnClick()
    {
        GameManager.Instance.TimeScale();
        if (GameManager.Instance.IsTimeScaled())
            ButtonOn();
        else
            ButtonOff();
    }
}
