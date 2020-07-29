using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    [SerializeField]
    private GameObject _laserPrefab;


    [SerializeField]
    private AudioClip explosionSound;

    private AudioSource audioSource;

    private Player _player;
    private Animator _anim;
    private float _fireRate = 3.0f;
    private float _canfire = -1f;
    private bool _isDead;
    // Update is called once per frame

    private void Start()
    {
        _isDead = false;
        _anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            UnityEngine.Debug.LogError("Audio source on enemy is null!");
        }
        else
        {
            audioSource.clip = explosionSound;
        }
        if (_anim == null)
        {
            Debug.LogError("Animator is NUll");
        }
        _player = GameObject.FindObjectOfType<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }

    }
    void Update()
    {
        CalculateMovement();
        if (_laserPrefab != null)
        {
            if (Time.time > _canfire)
            {
                _fireRate = Random.Range(3f, 7f);
                _canfire = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
          
                foreach (Laser l in lasers)
                {
                    l.AssignEnemyLaser();
                }
          
            }
        }
            
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime, 0);
        //if at botom respawn at top with new random x postion
        if (transform.position.y < -6f && _isDead == false)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {            
            if(_player != null)
            {
                _player.Damage();
            }
            Die();
        }
        else if(other.gameObject.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {  
                _player.AddScore(10);
            }
            Die();           
        }        
    }

    private void Die()
    {
        _isDead = true;
        _speed = 1f;
        audioSource.Play();
        Destroy(this.gameObject.GetComponent<Collider2D>());
        if (_anim != null)
        {
            _anim.SetTrigger("OnEnemyDeath");
        }
       
        Destroy(this.gameObject, _anim.GetCurrentAnimatorStateInfo(0).length); ;
    }
}
