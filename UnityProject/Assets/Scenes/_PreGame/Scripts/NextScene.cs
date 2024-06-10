using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    private void Start()
    {
        SceneManagarGame.Instance.NextScene(0);
    }
}
