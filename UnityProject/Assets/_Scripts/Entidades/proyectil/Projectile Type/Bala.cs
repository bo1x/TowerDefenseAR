using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bala", menuName = "ScriptableObjects/Bala", order = 1)]
public class Bala : ScriptableObject

{
    public Mesh Modelo3D;
    public Material MaterialDeLaBala;

    public float Damage;

    public float TiempoDeVida;

    public Vector3 scale;

    public Vector3 TamanyoDeLaBala;

    public Vector3 TamanyoEnArea;

    public GameObject particula;

    //Discutir con alex
    public float Velocidad;
}
