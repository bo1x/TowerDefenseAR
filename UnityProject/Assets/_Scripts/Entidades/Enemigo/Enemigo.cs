using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Enemigo : entidades
{
    [SerializeField] private ScriptableEnemigos _enemigo;
    [SerializeField] private GameObject target;
    [SerializeField] Material[] materials;
    [SerializeField] private Slider _slider;

    bool IsDestroying = false;

    private void Start()
    {
        target = GameManager.Instance.GetBase().gameObject;
        vida = _enemigo.Vida;
        _slider.maxValue = _enemigo.Vida;
        _slider.value = _enemigo.Vida;

        //ChangeColor();
        transform.LookAt(target.transform.position);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 distance = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * _enemigo.Velocidad);
        transform.position = distance;
        
    }

    public float GetMaxHealth()
    {
        return _enemigo.Vida;
    }

    public override void DoDamage(float value)
    {
        if (IsDestroying) return;

        base.DoDamage(value);
        SetSliderValue();
        if (vida <= 0)
            DestroyWithBonus();
    }

    private void DestroyWithBonus()
    {
        GameManager.Instance.AddMoney(_enemigo.Vida);
        Destroy();
    }

    void ChangeColor()
    {
        foreach(Material material in materials)
        {
            material.color = _enemigo.Color;
        }
    }

    private void SetSliderValue()
    {
        _slider.value = Mathf.Clamp(vida, 0, _enemigo.Vida);
    }

    public void Destroy()
    {
        GameManager.Instance.SetEnemyNumber(GameManager.Instance.GetEnemyNumber() - 1);

        IsDestroying = true;

        //animaciones con while
        Destroy(gameObject);
        return;
    }
}
