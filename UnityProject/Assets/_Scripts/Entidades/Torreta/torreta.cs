using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class torreta : MonoBehaviour
{
    [SerializeField] ScriptableTorreta _torreta;
    [SerializeField] private Transform[] Canyones;
    [SerializeField] private GameObject _AreaVisual;

    float cooldownTimestamp;
    bool canShoot;

    GameObject Enemie;

    [SerializeField] private OutlineLine Line;

    // Start is called before the first frame update
    void Start()
    {
        AreaUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Line.CanShoot)
            return;

        if (Enemie != null)
            transform.LookAt(new Vector3(Enemie.transform.position.x, Enemie.transform.position.y, Enemie.transform.position.z));

        if (TryShoot())
        {
            foreach(Transform canyon in Canyones)
            {
                Shoot(canyon);
            }
        }
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

        cooldownTimestamp = Time.time + _torreta.FireRate;
        return true;
    }

    private bool inArea()
    {
        Collider[] EnemyList = Physics.OverlapSphere(transform.position, _torreta.Rango, GameManager.Instance.GetLayerMask());

        if (EnemyList.Length - 1 < 0)
            return false;

        float Distance = (Enemie == null) ? 0 : (Mathf.Abs(Enemie.transform.position.x - GameManager.Instance.GetBase().transform.position.x) + Mathf.Abs(Enemie.transform.position.z - GameManager.Instance.GetBase().transform.position.z));
        bool isReset = false;

        foreach (Collider c in EnemyList)
        {

            if (Enemie == null)
            {
                Enemie = c.gameObject;
                Distance = Mathf.Abs(Enemie.transform.position.x - GameManager.Instance.GetBase().transform.position.x) + Mathf.Abs(Enemie.transform.position.z - GameManager.Instance.GetBase().transform.position.z);
                isReset = true;
                continue;
            }

            if (Enemie.name != c.gameObject.name)
            {
                float newDistance = Mathf.Abs(c.transform.position.x - GameManager.Instance.GetBase().transform.position.x) + Mathf.Abs(c.transform.position.z - GameManager.Instance.GetBase().transform.position.z);
                if (Distance > newDistance)
                {
                    Enemie = c.gameObject;
                    isReset = true;
                    Distance = newDistance;
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
        GameObject GO;
        GO = new GameObject("Bala de " + gameObject.name);
        GO.transform.rotation = gameObject.transform.rotation;
        proyectil proyectilscript = GO.AddComponent<proyectil>();
        proyectilscript._bala = _torreta.ScriptProyectil;
    }

    public void Shoot(Transform value)
    {
        transform.LookAt(new Vector3(Enemie.transform.position.x, transform.position.y, Enemie.transform.position.z));
        Debug.Log(Enemie.name);
        GameObject GO;
        GO = new GameObject("Bala de " + gameObject.name);
        GO.transform.position = value.position;
        GO.transform.rotation = value.rotation;
        GO.transform.localScale = _torreta.ScriptProyectil.scale;
        proyectil proyectilscript = GO.AddComponent<proyectil>();
        proyectilscript._bala = _torreta.ScriptProyectil;
    }

    private void AreaUpdate()
    {
        _AreaVisual.transform.localScale = new Vector3((_torreta.Rango * 2) / transform.parent.transform.localScale.z, (_torreta.Rango * 2) / transform.parent.transform.localScale.y, (_torreta.Rango * 2) / transform.parent.transform.localScale.z);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = Color.yellow;
        
    }
}
