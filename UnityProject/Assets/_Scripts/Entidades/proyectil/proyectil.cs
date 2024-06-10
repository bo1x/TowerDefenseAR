using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proyectil : entidades
{
    [SerializeField] public Bala _bala;
    bool isdestroying = false;
    GameObject enemigo;
    float WhereInst;

    private void Awake()
    {
        WhereInst = Mathf.Abs(transform.position.x) + Mathf.Abs(transform.position.z);
    }

    private void Start()
    {
        if (_bala.Modelo3D != null)
        {
            MeshFilter Mesh = gameObject.AddComponent<MeshFilter>();
            Mesh.mesh = _bala.Modelo3D;
            MeshRenderer meshRender = gameObject.AddComponent<MeshRenderer>();
            meshRender.material = _bala.MaterialDeLaBala;
        }
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        //Tambien se puede destruir dependiendo de las animaciones entregadas
        if (isdestroying)
            return;

        float normal = Mathf.Abs(transform.position.z) + Mathf.Abs(transform.position.x);

        if (ishit() || Mathf.Abs(normal) > Mathf.Abs(WhereInst) + Mathf.Abs(_bala.TiempoDeVida))
            destroy();

        moveBullet();
    }

    private void moveBullet()
    {
        gameObject.transform.Translate(Vector3.forward * _bala.Velocidad * Time.deltaTime);
    }

    private bool ishit()
    {
        Collider[] ishitted;
        ishitted = Physics.OverlapBox(gameObject.transform.position, (_bala.TamanyoDeLaBala/2), transform.rotation, GameManager.Instance.GetLayerMask());

        if (ishitted.Length - 1 < 0)
            return false;

        foreach (Collider collider in ishitted)
        {
            if (collider.GetComponent<Enemigo>() != null)
            {
                enemigo = ishitted[0].gameObject;
                return true;
            }
        }
        return false;
    }

    private void destroy()
    {
        isdestroying = true;
        if (enemigo != null)
        {
            if(enemigo.GetComponent<Enemigo>().vida > 0)
                BulletDoDamage();
            else
            {
                isdestroying = false;
                return;
            }
        }
            
        if (!_bala.TamanyoEnArea.Equals(Vector3.zero))
            danyoenarea();

        //Hace daño XD
        //Hace animaciones con un while
        Destroy(gameObject);

    }

    private void danyoenarea()
    {
        Instantiate(_bala.particula, transform.position, Quaternion.identity);
        Collider[] ishitted;
        ishitted = Physics.OverlapBox(gameObject.transform.position, (_bala.TamanyoEnArea / 2), transform.rotation, GameManager.Instance.GetLayerMask());
        GameObject firstenemigo = enemigo;
        foreach(Collider enemy in ishitted)
        {
            if (enemy == firstenemigo)
                continue;

            enemigo = enemy.gameObject;
            BulletDoDamage();
        }
    }
    
    void BulletDoDamage()
    {
        Enemigo _enemyscript = enemigo.GetComponent<Enemigo>();

        if (_enemyscript != null)
            _enemyscript.DoDamage(_bala.Damage);
        else
            Debug.LogError("Hay un enemigo que ha sido golpeado con la bala que no tiene script de 'Enemigo'");
    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, _bala.TamanyoDeLaBala);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, _bala.TamanyoEnArea);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, _bala.scale);
    }

}
