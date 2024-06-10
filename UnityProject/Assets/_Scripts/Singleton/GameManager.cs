using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Xml.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _Camera;
    public static GameManager Instance { get; private set; }

    [SerializeField] private Base mybase;
    private float VidaMaximBase = 50    ;

    [SerializeField] private TMP_Text _HealthText;
    [SerializeField] private LayerMask enemyLayerMask;
    private RoundManager _roundmanager;
    private float EnemyNumber = 0;
    public float maxRound{ get; private set;}
    private float round = 1;
    [SerializeField] private TMP_Text _RoundText;

    private bool _StopWave = true;
    private float _MaxTimeSpeed = 2;
    private bool isTimeScaled;


    private float Money = 0;
    private float Roundincome = 100;
    [SerializeField] private TMP_Text _MoneyText;

    //UI
    [SerializeField] Button BotonDePlay;
    [SerializeField] Button BotonDeBase;
    [SerializeField] Button BotonDeSpawn;
    [SerializeField] Button BotonDeVelocidad;

    [SerializeField] Button BotonDeTorretaSimple;
    [SerializeField] Button BotonDeTriple;
    [SerializeField] Button BotonDeInfernal;
    [SerializeField] Button BotonDeFire;
    [SerializeField] Button[] BotonDeTorreta;
    [SerializeField] float[] PreciosDeTorreta;
    [SerializeField] float[] RondaDeTorreta;
    [SerializeField] GameObject[] botonCandado;
    [SerializeField] GameObject[] precio;

    //UI Victory Derrota
    [SerializeField] Canvas CANVASUI;
    [SerializeField] Canvas Victory;
    [SerializeField] Canvas Defeat;


    private Transform selection;
    private Transform highlight;
    private RaycastHit raycastHit;

    [SerializeField] LineRenderer PathLine;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        BotonDeVelocidad.gameObject.SetActive(false);
        BotonDePlay.interactable = false;
        maxRound = 10;
        AddMoney(650);
        UpdateMoney();
        _HealthText.text = VidaMaximBase.ToString();
        _RoundText.text = 1 + "/" + maxRound;
    }

    // Update is called once per frame
    void Update()
    {
        selectionItem();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }

    public Transform GetCamera()
    {
        return _Camera;
    }

    public void SetBase(Base value)
    {
        mybase = value;
    }

    public Base GetBase() { return mybase; }

    public float GetVidaMaxima()
    {
        return VidaMaximBase;
    }

    public void CheckButton()
    {
        if (_roundmanager != null && _roundmanager.gameObject.activeSelf && mybase != null && mybase.gameObject.activeSelf)
        {
            BotonDePlay.interactable = true;
            PathLine.gameObject.SetActive(true);
        }
    }

    public void DisablePlayButton()
    {
        BotonDePlay.interactable = false;
        PathLine.gameObject.SetActive(false);
    }

    public void UpdateHealth()
    {
        _HealthText.text = mybase.GetHealth().ToString();
    }

    public LayerMask GetLayerMask() {  return enemyLayerMask; }

    #region Rounds

    public void RoundUpdate()
    {
        _RoundText.text = round + "/" + maxRound;
    }

    public void SetEnemyNumber(float value)
    {
        EnemyNumber = value;
        if (value != 0) 
            return;

        FinishRound();
    }

    public float GetEnemyNumber() {
        return EnemyNumber;
    }

    public void SetRound(float value)
    {
        round = value;
    }

    public float GetRound()
    {
        return round;
    }

    public void SetRoundManager(RoundManager value)
    {
        _roundmanager = value;
    }

    public RoundManager GetRoundManager() {
        return _roundmanager;
    }
    #endregion

    #region Play

    public void FinishRound()
    {
        round++;
        UpdateTorretaButtons();
        if (!_StopWave || maxRound < round)
        {
            _roundmanager.NextRound();
            return;
        }
        PathLine.gameObject.SetActive(true);
        BotonDePlay.gameObject.SetActive(true);
        BotonDeVelocidad.gameObject.SetActive(false);
    }

    public void LineUpdate()
    {
        if (mybase == null || _roundmanager == null)
            return;

        PathLine.gameObject.SetActive(true);
        PathLine.SetPosition(1, mybase.transform.position);
        PathLine.SetPosition(0, _roundmanager.transform.position);
    }

    public GameObject GetPathLine()
    {
        return PathLine.gameObject;
    }



    public void NextRound()
    {
        BotonDePlay.gameObject.SetActive(false);
        BotonDeVelocidad.gameObject.SetActive(true);
        GetRoundManager().NextRound();

        if (round != 1)
            return;

        BotonDeBase.interactable = false;
        BotonDeSpawn.interactable = false;
    }

    public void TimeScale()
    {
        isTimeScaled = !isTimeScaled;
        Time.timeScale = isTimeScaled ? _MaxTimeSpeed : 1;
    }

    public bool IsTimeScaled()
    {
        return isTimeScaled;
    }

    #endregion

    #region Win Lose

    public void Lose()
    {
        AudioManager.Instance.Play("GameOver");
        CANVASUI.gameObject.SetActive(false);
        Victory.gameObject.SetActive(false);
        Defeat.gameObject.SetActive(true);
    }

    public void Win()
    {
        AudioManager.Instance.Play("Victory");
        CANVASUI.gameObject.SetActive(false);
        Defeat.gameObject.SetActive(false);
        Victory.gameObject.SetActive(true);
    }

    #endregion

    #region Money

    public float GetMoney()
    {
        return Money;
    }

    public void SetMoney(float value)
    {
        Money = value;
        UpdateMoney();
        return;
    }

    public void AddMoney(float value)
    {
        Money += value;
        UpdateMoney();
        return;
    }

    public void Roundmoney()
    {
        if (round < 1)
            return;

        AddMoney(Roundincome);
    }

    private void UpdateMoney()
    {
        _MoneyText.text = Money.ToString();
        UpdateTorretaButtons();
    }

    private void UpdateTorretaButtons()
    {
        int i = 0;
        foreach (Button boton in BotonDeTorreta)
        {
            boton.interactable = (Money >= PreciosDeTorreta[i] && RondaDeTorreta[i] <= round) ? true : false;
            if (botonCandado[i] != null && boton.interactable)
            {
                botonCandado[i].SetActive(false);
                precio[i].SetActive(true);
            }
            i++;
        }
    }

    public float GetPrice(int index)
    {
        return PreciosDeTorreta[index];
    }

    #endregion

    #region Selector

    void selectionItem()
    {
        Debug.Log(highlight);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            if (!highlight.CompareTag("Torreta"))
            {
                highlight = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!highlight)
            {
                if (selection)
                {
                    selection.gameObject.GetComponentInParent<OutlineLine>().enabled = false;
                    selection = null;
                }
                return;
            }
            else
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponentInParent<OutlineLine>().enabled = false;
                    if(selection == highlight)
                    {
                        selection = null;
                        highlight = null;
                        return;
                    }
                }
                selection = raycastHit.transform;
                //Esta linea da fallo
                if(selection?.gameObject?.GetComponentInParent<OutlineLine>() != null)
                    selection.gameObject.GetComponentInParent<OutlineLine>().enabled = true;

                highlight = null;
            }
        }
    }
    #endregion
}
