using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class FruitController : MonoBehaviourPun
{
    [SerializeField] private GameObject _fruitSpawns;
    [SerializeField] private GameObject _spawnedFruits;
    [SerializeField] private GameObject[] _fruits;
    [SerializeField] private float _fruitDuration;
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _countDown;
    [SerializeField] private bool _spawning;

    // Start is called before the first frame update
    void Awake()
    {
        _countDown = _spawnRate;
        _spawning = false;
        Fruit.OnDropped += HandleFruitDropped;
    }
    private void OnDestroy()
    {
        Fruit.OnDropped -= HandleFruitDropped;
    }

    private void HandleFruitDropped(int playerID, int amount)
    {
        for (int i = 0; i < Mathf.Abs(amount); i++)
        {
            SpawnFruit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.IsMasterClient && _spawning)
        {
            _countDown -= Time.deltaTime;

            if (_countDown <= 0)
            {
                SpawnFruit();
                _countDown = _spawnRate;
            }
        }
    }

    public void StartSpawning()
    {
        _spawning = true;
        _countDown = _spawnRate;
    }
    public void EndSpawning()
    {
        _spawning = false;
        _countDown = _spawnRate;
        StopAllCoroutines();
    }

    private void SpawnFruit()
    {
        int randomSpawn = UnityEngine.Random.Range(0, _fruitSpawns.transform.childCount);
        int randomFruit = UnityEngine.Random.Range(0, _fruits.Length);

        Vector3 spawnPoint = _fruitSpawns.transform.GetChild(randomSpawn).transform.position;
        GameObject fruit = _fruits[randomFruit];

        GameObject newFruit = PhotonNetwork.Instantiate(fruit.name, spawnPoint, Quaternion.identity);
        newFruit.transform.SetParent(_spawnedFruits.transform);
        StartCoroutine("DestoryFruit", newFruit);
    }

    private IEnumerator DestoryFruit(GameObject fruit)
    {
        yield return new WaitForSeconds(_fruitDuration);
        if (fruit.gameObject != null)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(fruit);
            }
        }
    }
}
