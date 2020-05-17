using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FruitController : MonoBehaviour
{
    [SerializeField] private GameObject _fruitSpawns;
    [SerializeField] private GameObject _spawnedFruits;
    [SerializeField] private GameObject[] _fruits;
    [SerializeField] private float _fruitDuration;
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _countDown;

    // Start is called before the first frame update
    void Start()
    {
        _countDown = _spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.IsMasterClient)
        {
            _countDown -= Time.deltaTime;

            if (_countDown <= 0)
            {
                SpawnFruit();
                _countDown = _spawnRate;
            }
        }
    }

    private void SpawnFruit()
    {
        int randomSpawn = Random.Range(0, _fruitSpawns.transform.childCount);
        int randomFruit = Random.Range(0, _fruits.Length);

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
            PhotonNetwork.Destroy(fruit);
        }
    }
}
