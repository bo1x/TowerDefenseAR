using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Lanzallamas : MonoBehaviour
{
    [SerializeField] private float _FireRate;
    [SerializeField] private float _Area;
    [SerializeField] private GameObject _AreaImage;
    [SerializeField] private ParticleSystem particle;

    [SerializeField] private Vector3 _FireArea;
    [SerializeField] private GameObject _FireCollider;
    [SerializeField] private float _FireHoldTime;

    float cooldownTimestamp;
    bool canShoot;

    GameObject Enemie;

    [SerializeField] private OutlineLine Line;

    // Start is called before the first frame update
    void Start()
    {
        particle.Stop();
        _FireCollider.transform.localEulerAngles = (new Vector3(0, 90, 0));
        _FireCollider.transform.localPosition = new Vector3(0, 0, _FireArea.x/2);
        _FireCollider.transform.localScale = _FireArea;
        _FireCollider.SetActive(false);
        AreaUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Line.CanShoot)
            return;

        if (Enemie != null)
            transform.LookAt(new Vector3(Enemie.transform.position.x, transform.position.y, Enemie.transform.position.z));

        if (TryShoot())
        {
            Shoot();
        }

    }

    private void FixedUpdate()
    {
    }

    public bool TryShoot()
    {
        if (!inArea())
        {
            Enemie = null;
            particle.Stop();
            return false;
        }

        if (Time.time < cooldownTimestamp || !canShoot)
        {
            canShoot = true;
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
                    Distance = newDistance;
                    isReset = true;
                    continue;
                }
            }
            else
            {
                isReset = true;
                continue;
            }
        }
        if(!particle.isEmitting)
            particle.Play();

        return isReset;
    }


    public void Shoot()
    {
        _FireCollider.SetActive(true);
        transform.LookAt(new Vector3(Enemie.transform.position.x, transform.position.y, Enemie.transform.position.z));
        Invoke("DisableFire", _FireHoldTime);
    }

    private void DisableFire()
    {
        _FireCollider.SetActive(false);
    }

    private void AreaUpdate()
    {
        _AreaImage.transform.localScale = new Vector3((_Area * 2) / transform.parent.transform.localScale.z, (_Area * 2) / transform.parent.transform.localScale.y, (_Area * 2) / transform.parent.transform.localScale.z);
    }
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
      /*  Gizmos.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, _Area);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y, transform.position.z + _FireArea.x / 2), new Vector3(_FireArea.z, _FireArea.y, _FireArea.x));
      */
    }
}
