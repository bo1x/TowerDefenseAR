using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TorretaPorcentual : MonoBehaviour
{
    [SerializeField] private LineRenderer _Ray;
    [SerializeField] private Transform _FirePoint;

    [SerializeField] private float _MaxDamage, _MinDamage;
    [SerializeField] private float _MaxSpeed;
    private float DamageStartTime;

    [SerializeField] private float _FireRate;
    [SerializeField] private float _Area;

    [SerializeField] private GameObject _AreaImage;

    float cooldownTimestamp;
    bool canShoot;

    [SerializeField] private LayerMask enemyLayerMask;
    GameObject Enemie;

    [SerializeField] private OutlineLine Line;

    private void Start()
    {
        AreaUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Line.CanShoot)
            return;

        if (TryShoot())
        {
                Shoot();
        }


    }

    private void FixedUpdate()
    {
        rayAnimation();
    }

    public bool TryShoot()
    {
        if (Time.time < cooldownTimestamp || !canShoot)
        {
            canShoot = true;
            return false;
        }

        if (!inArea())
        {
            Enemie = null;
            return false;
        }

        cooldownTimestamp = Time.time + _FireRate;
        return true;
    }

    private bool inArea()
    {
        Collider[] EnemyList = Physics.OverlapSphere(transform.position, _Area, GameManager.Instance.GetLayerMask());

        if (EnemyList.Length - 1 < 0)
            return false;

        float MaxHealth = (Enemie == null) ? 0 : Enemie.GetComponent<Enemigo>().GetMaxHealth();
        bool isReset = false;

        foreach (Collider c in EnemyList)
        {

            if (Enemie == null)
            {
                Enemie = c.gameObject;
                MaxHealth = Enemie.GetComponent<Enemigo>().GetMaxHealth();
                ResetDamage();
                isReset = true;
                continue;
            }

            if (Enemie.name != c.gameObject.name)
            {
                float newHealth = c.gameObject.GetComponent<Enemigo>().GetMaxHealth();
                if (MaxHealth < newHealth)
                {
                    Enemie = c.gameObject;
                    MaxHealth = newHealth;
                    isReset = true;
                    ResetDamage();
                    continue;
                }
            }
            else
            {
                isReset = true;
                continue;
            }
        }

        return isReset;
    }

    void Shoot()
    {
        canShoot = false;
        Enemigo _enemyscript = Enemie.GetComponent<Enemigo>();

        if (_enemyscript != null)
        {
            _enemyscript.DoDamage(DamageCalculation());
        }
        else
            Debug.LogError("Hay un enemigo que ha sido golpeado con la bala que no tiene script de 'Enemigo'");
    }

    float DamageCalculation()
    {
       return Mathf.Lerp(_MinDamage, _MaxDamage, (Time.time - DamageStartTime) * _MaxSpeed);
    }

    void ResetDamage()
    {
        DamageStartTime = Time.time;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = Color.yellow;
      
    }


    void rayAnimation()
    {
        _Ray.gameObject.transform.eulerAngles = Vector3.zero;
        if (Enemie == null)
        {
            _Ray.gameObject.SetActive(false);
            return;
        }

        _Ray.gameObject.SetActive(true);
        _Ray.SetPosition(0, _FirePoint.position - transform.position);
        Vector3 TranslatedFirePoint = Enemie.transform.position - (transform.position);
        _Ray.SetPosition(1, new Vector3(TranslatedFirePoint.x, TranslatedFirePoint.y, TranslatedFirePoint.z));
    }

    private void AreaUpdate()
    {
        _AreaImage.transform.localScale = new Vector3((_Area * 2) / transform.parent.transform.localScale.z, (_Area * 2) / transform.parent.transform.localScale.y, (_Area * 2) / transform.parent.transform.localScale.z);
    }
}
