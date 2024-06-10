using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;

    private GameObject enemytoSpawn;

    int enemyNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetRoundManager(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextRound()
    {
        GameManager.Instance.Roundmoney();
        if (GameManager.Instance.GetRound() > GameManager.Instance.maxRound)
        {
            GameManager.Instance.Win();
            return;
        }
        GameManager.Instance.RoundUpdate();
        Invoke("Round" + GameManager.Instance.GetRound(),0);
    }

    #region Rondas

    private void Round1()
    {
        GameManager.Instance.SetEnemyNumber(20);
        SpawnEnemyMassive(enemyPrefab[0], 2, 20);
    }

    private void Round2()
    {
        GameManager.Instance.SetEnemyNumber(35);
        SpawnEnemyMassive(enemyPrefab[0], 2, 35);
    }

    private void Round3()
    {
        GameManager.Instance.SetEnemyNumber(30);
        StartCoroutine(Round3Routine());

    }

    IEnumerator Round3Routine()
    {
        SpawnEnemyMassive(enemyPrefab[0], 1, 10);
        yield return new WaitForSeconds(1 * 10);
        SpawnEnemyMassive(enemyPrefab[1], 1, 5);
        yield return new WaitForSeconds(1 * 5);
        SpawnEnemyMassive(enemyPrefab[0], 1, 15);
    }

    private void Round4()
    {
        GameManager.Instance.SetEnemyNumber(53);
        StartCoroutine (Round4Routine());
    }

    IEnumerator Round4Routine()
    {
        SpawnEnemyMassive(enemyPrefab[0], 1.5f, 15);
        yield return new WaitForSeconds(1.5f * 15);
        SpawnEnemyMassive(enemyPrefab[0], 1.5f, 10);
        SpawnEnemyMassive(enemyPrefab[1], 1f, 18);
        yield return new WaitForSeconds((1.5f * 10) + 1);
        SpawnEnemyMassive(enemyPrefab[0], 1.5f, 10);
    }

    private void Round5()
    {
        GameManager.Instance.SetEnemyNumber(32);
        StartCoroutine(Round5Routine());
    }

    IEnumerator Round5Routine()
    {
        SpawnEnemyMassive(enemyPrefab[1], 1.5f, 10);
        yield return new WaitForSeconds(1.5f * 10);
        SpawnEnemyMassive(enemyPrefab[0], 1.5f, 5);
        yield return new WaitForSeconds(1.5f * 5);
        SpawnEnemyMassive(enemyPrefab[1], 1.5f, 17);
    }

    private void Round6()
    {
        GameManager.Instance.SetEnemyNumber(34);
        StartCoroutine(Round6Routine());
    }

    IEnumerator Round6Routine()
    {
        SpawnEnemyMassive(enemyPrefab[2], 1.5f, 4);
        yield return new WaitForSeconds((1.5f * 4) + 1.5f);
        SpawnEnemyMassive(enemyPrefab[0], 1, 15);
        yield return new WaitForSeconds(1 * 15);
        SpawnEnemyMassive(enemyPrefab[1], 1, 15);
    }
   

    private void Round7()
    {
        GameManager.Instance.SetEnemyNumber(45);
        StartCoroutine(Round7Routine());
    }

    IEnumerator Round7Routine()
    {
        SpawnEnemyMassive(enemyPrefab[1], 1.5f, 10);
        yield return new WaitForSeconds((1.5f * 10) + 0.5f);
        SpawnEnemyMassive(enemyPrefab[2], 1.5f, 5);
        yield return new WaitForSeconds((1 * 5) + 1f);
        SpawnEnemyMassive(enemyPrefab[0], 1.5f, 20);
        yield return new WaitForSeconds((1 * 20));
        SpawnEnemyMassive(enemyPrefab[1], 1, 10);
    }

    private void Round8()
    {
        GameManager.Instance.SetEnemyNumber(45);
        StartCoroutine(Round8Routine());
    }

    IEnumerator Round8Routine()
    {
        SpawnEnemyMassive(enemyPrefab[1], 1.5f, 20);
        yield return new WaitForSeconds((1.5f * 20) + 0.5f);
        SpawnEnemyMassive(enemyPrefab[2], 1, 5);
        yield return new WaitForSeconds((1 * 4) + 1f);
        SpawnEnemyMassive(enemyPrefab[0], 0.5f, 10);
        yield return new WaitForSeconds(((0.5f * 10) + 1));
        SpawnEnemyMassive(enemyPrefab[2], 1, 10);
    }

    private void Round9()
    {
        GameManager.Instance.SetEnemyNumber(30);
        SpawnEnemyMassive(enemyPrefab[2], 1.5f, 30);
    }

    private void Round10()
    {
        GameManager.Instance.SetEnemyNumber(102);
        StartCoroutine(Round10Routine());
    }

    IEnumerator Round10Routine()
    {
        SpawnEnemyMassive(enemyPrefab[1], 1.5f, 34);
        yield return new WaitForSeconds((1.5f * 34));
        SpawnEnemyMassive(enemyPrefab[1], 1, 34);
        yield return new WaitForSeconds((1 * 34));
        SpawnEnemyMassive(enemyPrefab[1], 0.5f, 34);
        yield return new WaitForSeconds((0.5f * 34));
    }

    #endregion


    #region Funciones para invocar XD
    void SpawnEnemyMassive(GameObject enemigo, float delay, int cicle)
    {
        for (int i = 0; i < cicle; i++)
        {
            enemytoSpawn = enemigo;
            Invoke("SpawnEnemy", delay * i);
        }
    }

    void SpawnEnemy()
    {
        GameObject Enemy = Instantiate(enemytoSpawn, transform.position, Quaternion.identity);
        Enemy.name = "Enemy " + enemyNumber;
        enemyNumber++;
    }
    #endregion
}
