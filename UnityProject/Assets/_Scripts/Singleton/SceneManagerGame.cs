using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneManagarGame : MonoBehaviour
{
    public static SceneManagarGame Instance { get; private set; }

    [SerializeField] private GameObject _loading;
    private Coroutine loading;
    private float minLoadTime = 0.3f;
    private bool isLoading = false;

    [SerializeField] private TMP_Text AnimationLoading;
    float LoadingTimeAnimation = 0.2f;
    Coroutine loadingRoutine;

    [SerializeField] private GameObject pauseprefab;
    private GameObject _pauseObject;
    private bool pause = false;

    [SerializeField] private Image _FadeImage;
    [SerializeField] private float _fadeTime = 0.2f;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene(int value)
    {
        switch (value)
        {
            case 0:
                LoadLevelUI("MainMenu");
                break;
            case 1:
                LoadLevelUI("ARscene");
                break;
            default:
                Debug.LogWarning("Esta escena no se reconoce");
                break;
        }
    }

    public void LoadLevelUI(string levelToLoad)
    {
        loading = StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        isLoading = true;
        _loading.transform.parent.gameObject.SetActive(true);

        _FadeImage.gameObject.SetActive(true);
        _FadeImage.canvasRenderer.SetAlpha(0);


        while (!Fade(1))
            yield return null;

        _loading.SetActive(true);

        loadingRoutine = StartCoroutine(Loading());

        while (!Fade(0))
            yield return null;

        if (levelToLoad == "Game")
            pauseprefab.SetActive(true);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);


        System.GC.Collect();

        float progressValue = 0f;

        while (!loadOperation.isDone)
        {
            progressValue += Time.deltaTime;
            yield return null;
        }

        while (progressValue < minLoadTime)
        {
            progressValue += Time.deltaTime;
            yield return null;
        }

        pauseprefab.SetActive(false);

        while (!Fade(1))
            yield return null;

        _loading.SetActive(false);

        StopCoroutine(loadingRoutine);

        while (!Fade(0))
            yield return null;

        _loading.transform.parent.gameObject.SetActive(false);
        isLoading = false;

        yield return null;
    }

    IEnumerator Loading()
    {
        while(true)
        {
            AnimationLoading.text = ".";
            yield return new WaitForSeconds(LoadingTimeAnimation);
            AnimationLoading.text = "..";
            yield return new WaitForSeconds(LoadingTimeAnimation);
            AnimationLoading.text = "...";
            yield return new WaitForSeconds(LoadingTimeAnimation);
        }
    }

    private bool Fade(float target)
    {
        _FadeImage.CrossFadeAlpha(target, _fadeTime, true);

        if (Mathf.Abs(_FadeImage.canvasRenderer.GetAlpha() - target) <= 0.05f)
        {
            _FadeImage.color = new Color(_FadeImage.color.r, _FadeImage.color.g, _FadeImage.color.b, target);
            return true;
        }

        return false;
    }

    public bool GetPause()
    {
        return pause;
    }

    public void Pause(bool value)
    {
        if (pause && value == false)
        {
            pause = false;
            pauseprefab.SetActive(false);
            return;
        }
        else if (value == false)
            return;

        pause = true;
        pauseprefab.SetActive(true);

    }

    public bool GetLoad()
    {
        return isLoading;
    }
}
