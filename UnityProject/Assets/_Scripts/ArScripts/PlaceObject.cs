using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    private PlaceIndicator placeIndicator;
    public GameObject torre1canon;
    public GameObject torre3canon;
    public GameObject torreInfernal;
    public GameObject torreMisiles;
    public GameObject torreLanzallamas;
    public GameObject base_;
    public GameObject spawnEnemigos;

    private GameObject newPlacedObject;


    private float TorretIndex = -1;

    public bool torretasPosicionadas = false;

    [SerializeField] private GameObject SetButton;

    /*private GameObject _torre1canon;
    private GameObject _torre3canon;
    private GameObject _torreInfernal;
    private GameObject _torreMisiles;
    private GameObject _torreLanzallamas;*/


    //Añadir BotonUI enemigos, poner tags y crear función
    // Start is called before the first frame update
    void Start()
    {
        placeIndicator = FindObjectOfType<PlaceIndicator>();
    }

    // Update is called once per frame
    void Update()
    {


   /*   _torre1canon         = GameObject.FindGameObjectWithTag("");
      _torre3canon         = GameObject.FindGameObjectWithTag("");
      _torreInfernal       = GameObject.FindGameObjectWithTag("");
      _torreMisiles        = GameObject.FindGameObjectWithTag("");
      _torreLanzallamas    = GameObject.FindGameObjectWithTag("");
      __base               = GameObject.FindGameObjectWithTag("");
      _spawnEnemigos       = GameObject.FindGameObjectWithTag("");

        */


        /*if (_torre1canon && _torre3canon && torreLanzallamas && _torreInfernal && _torreMisiles)
        {
            torretasPosicionadas=true;
        }
        else
        {
            torretasPosicionadas =false;
        }*/
    }

    public void ponerTorreta1canon()
    {
        AudioManager.Instance.Play("Click");
        if (newPlacedObject?.name == torre1canon.name + "(Clone)")
        {
            disableBuy();
            return;
        }

        DelelteLast();
        newPlacedObject = Instantiate(torre1canon, placeIndicator.transform.position, placeIndicator.transform.rotation);
        newPlacedObject.GetComponent<OutlineLine>().enabled = true;
        newPlacedObject.transform.parent = placeIndicator.transform;
        SetButton.SetActive(true);
        TorretIndex = 0;
    }
    public void ponerTorreta3canon()
    {
        AudioManager.Instance.Play("Click");
        if (newPlacedObject?.name == torre3canon.name + "(Clone)")
        {
            disableBuy();
            return;
        }

        DelelteLast();
        newPlacedObject = Instantiate(torre3canon, placeIndicator.transform.position, placeIndicator.transform.rotation);
        newPlacedObject.GetComponent<OutlineLine>().enabled = true;
        newPlacedObject.transform.parent = placeIndicator.transform;
        SetButton.SetActive(true);
        TorretIndex = 1;
    }
    public void ponerTorretaLanzallamas()
    {
        AudioManager.Instance.Play("Click");
        if (newPlacedObject?.name == torreLanzallamas.name + "(Clone)")
        {
            disableBuy();
            return;
        }

        DelelteLast();
        newPlacedObject = Instantiate(torreLanzallamas, placeIndicator.transform.position, placeIndicator.transform.rotation);
        newPlacedObject.GetComponent<OutlineLine>().enabled = true;
        newPlacedObject.transform.parent = placeIndicator.transform;
        SetButton.SetActive(true);
        TorretIndex = 2;
    }
    public void ponerTorretaInfernal()
    {
        AudioManager.Instance.Play("Click");
        if (newPlacedObject?.name == torreInfernal.name + "(Clone)")
        {
            disableBuy();
            return;
        }

        DelelteLast();
        newPlacedObject = Instantiate(torreInfernal, placeIndicator.transform.position, placeIndicator.transform.rotation);
        newPlacedObject.GetComponent<OutlineLine>().enabled = true;
        newPlacedObject.transform.parent = placeIndicator.transform;
        SetButton.SetActive(true);
        TorretIndex = 3;

    }

    public void ponerTorretaLanzamisiles()
    {
        AudioManager.Instance.Play("Click");
        if (newPlacedObject?.name == torreMisiles.name + "(Clone)")
        {
            disableBuy();
            return;
        }

        DelelteLast();
        newPlacedObject = Instantiate(torreMisiles, placeIndicator.transform.position, placeIndicator.transform.rotation);
        newPlacedObject.GetComponent<OutlineLine>().enabled = true;
        newPlacedObject.transform.parent = placeIndicator.transform;
        SetButton.SetActive(true);
        TorretIndex = 4;

    }

    public void ponerBase()
    {
        AudioManager.Instance.Play("Click");
        TorretIndex = -1;
        if (newPlacedObject == GameManager.Instance.GetBase()?.gameObject && GameManager.Instance.GetBase() != null)
        {
            disableBuy();
            return;
        }

        GameManager.Instance.GetPathLine().SetActive(false);

        DelelteLast();
        if (null != GameManager.Instance.GetBase())
        {
            newPlacedObject = GameManager.Instance.GetBase().gameObject;
            newPlacedObject.transform.position = placeIndicator.transform.position;
            newPlacedObject.transform.parent = placeIndicator.transform;
            newPlacedObject.SetActive(true);
            GameManager.Instance.DisablePlayButton();
            SetButton.SetActive(true);
        }
        else
        {
            newPlacedObject = Instantiate(base_, placeIndicator.transform.position, placeIndicator.transform.rotation);
            newPlacedObject.transform.parent = placeIndicator.transform;
            SetButton.SetActive(true);
        }
    }

    public void ponerEnemigos()
    {
        AudioManager.Instance.Play("Click");
        TorretIndex = -1;
        if (newPlacedObject == GameManager.Instance.GetRoundManager()?.gameObject && GameManager.Instance.GetRoundManager() != null)
        {
            disableBuy();
            return;
        }

        GameManager.Instance.GetPathLine().SetActive(false);

        DelelteLast();
        if (null != GameManager.Instance.GetRoundManager())
        {
            newPlacedObject = GameManager.Instance.GetRoundManager().gameObject;
            newPlacedObject.transform.position = placeIndicator.transform.position;
            newPlacedObject.transform.parent = placeIndicator.transform;
            GameManager.Instance.DisablePlayButton();
            newPlacedObject.SetActive(true);
            SetButton.SetActive(true);
        }
        else
        {
            newPlacedObject = Instantiate(spawnEnemigos, placeIndicator.transform.position, placeIndicator.transform.rotation);
            newPlacedObject.transform.parent = placeIndicator.transform;
            SetButton.SetActive(true);
        }
    }

    private void DelelteLast()
    {
        if (newPlacedObject == null)
            return;

        newPlacedObject.SetActive(false);
        if (newPlacedObject == GameManager.Instance.GetBase()?.gameObject || newPlacedObject == GameManager.Instance.GetRoundManager()?.gameObject)
        {
            newPlacedObject = null;
            return;
        }
        Destroy(newPlacedObject);
        newPlacedObject = null;
    }

    private void disableBuy()
    {
        SetButton.SetActive(false);
        DelelteLast();
    }

    public void SetObject()
    {
        if (newPlacedObject != null)
        {
            AudioManager.Instance.Play("TorretPlace");
            if (TorretIndex > -1)
            {
                GameManager.Instance.AddMoney(-GameManager.Instance.GetPrice((int)TorretIndex));
            }
            newPlacedObject.transform.parent = null;
            if (newPlacedObject?.GetComponent<OutlineLine>() != null)
            {
                newPlacedObject.GetComponent<OutlineLine>().CanShoot = true;
                newPlacedObject.GetComponent<OutlineLine>().enabled = false;
            }
            GameManager.Instance.LineUpdate();
            if (TorretIndex <= -1)
            {
                GameManager.Instance.CheckButton();
            }
            newPlacedObject = null;
            SetButton.SetActive(false);
            TorretIndex = -1;
        }
    }
}
