using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy;

    [SerializeField]
    private Powerup[] _powerups; 


    [SerializeField]
    private GameObject _enemyContainer;
    
    [SerializeField]
    private float _waitTime = 5f;

    [SerializeField]
    private int _maxEnemies = 20;

    [SerializeField]
    private Asteroid _asteroid;

    private bool _stopSpawning = false;
    private float _canSpawn = -1f;
    // Start is called before the first frame update
    void Start()
    {
        Asteroid asteroid = Instantiate(_asteroid, new Vector3(0, 3f, 0), Quaternion.identity);
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
             
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;       
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while(_stopSpawning == false)
        {            
            _canSpawn = Time.time + _waitTime;
            Enemy newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-9f, 9f), 7f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitUntil(()=>(Time.time > _canSpawn && GameObject.FindGameObjectsWithTag("Enemy").Length < _maxEnemies));            
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            Powerup newPowerup = Instantiate(_powerups[Random.Range(0,_powerups.Length)], new Vector3(Random.Range(-9f, 9f), 7f, 0), Quaternion.identity);
        }
    }
}
