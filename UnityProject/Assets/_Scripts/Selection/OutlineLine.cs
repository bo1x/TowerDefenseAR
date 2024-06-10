using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineLine : MonoBehaviour
{
    [SerializeField] MeshRenderer area;
    public bool CanShoot;
    private void OnEnable()
    {
        area.enabled = true;
        Debug.Log(gameObject.name);
    }

    private void OnDisable()
    {
        area.enabled = false;
        Debug.Log("chao");
    }
}
