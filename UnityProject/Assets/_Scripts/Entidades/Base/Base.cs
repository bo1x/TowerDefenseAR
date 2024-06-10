using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : entidades
{
    bool isLosing = false;
    // Start is called before the first frame update
    void Awake()
    {
        vida = GameManager.Instance.GetVidaMaxima();
    }

    private void Start()
    {
        GameManager.Instance.SetBase(this);
        GameManager.Instance.UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DoDamage(float value)
    {
        base.DoDamage(value);
        GameManager.Instance.UpdateHealth();
        if (vida <= 0)
            Destroy();
    }

    public float GetHealth() { return vida; }
    
    void Destroy()
    {
        if (isLosing)
            return;

        isLosing = true;
        GameManager.Instance.Lose();
        //animaciones con while
        //Destroy(gameObject);
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemigo>())
        {
            DoDamage(1);
            other.GetComponent<Enemigo>().Destroy();
        }
    }
}
