using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayAgain : MonoBehaviour
{
    
    public void menu(int value)
    {
        SceneManagarGame.Instance.NextScene(value);
    }
}
