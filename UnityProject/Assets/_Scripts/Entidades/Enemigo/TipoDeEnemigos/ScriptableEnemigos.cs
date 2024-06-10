using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemigos", menuName = "ScriptableObjects/Enemigos", order = 1)]
public class ScriptableEnemigos : ScriptableObject

{
    public float Danyo;

    public float Velocidad;

    public float Vida;

    public Color Color;
}
