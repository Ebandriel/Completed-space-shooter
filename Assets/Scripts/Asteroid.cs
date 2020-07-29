using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 2f;
    [SerializeField]
    private GameObject _explosionPrefab;

 

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        
        _spawnManager = FindObjectOfType<SpawnManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.25f);


        }
    }
}
