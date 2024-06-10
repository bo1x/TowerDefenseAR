using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entidades : MonoBehaviour
{
    public float vida;

    public virtual void DoDamage(float value)
    {
        vida = vida - value;
    }
}
