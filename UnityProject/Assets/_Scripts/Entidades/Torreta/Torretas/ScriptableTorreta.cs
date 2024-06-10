using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemigos", menuName = "ScriptableObjects/Torreta", order = 1)]
public class ScriptableTorreta : ScriptableObject

{
    public float FireRate;

    public float Rango;

    public Bala ScriptProyectil;
}
