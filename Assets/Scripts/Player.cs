using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public or private references
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 2f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _trippleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private  AudioClip _LaserShotSound;

    [SerializeField]
    private AudioClip _pickupSound;



    private SpawnManager _spawnManager;
    private float _canFire = -1f;

    //power up flags

    private bool _trippleShotActive = false;
    private float _trippleShotActiveTime = 5f;
    private float _trippleShotDeactivate = -1f;


    private bool _speedActive = false;
    private float _speedActiveTime = 6f;
    private float _speedDeactivate = -1f;

    [SerializeField]
    private bool _shieldActive = false;

    [SerializeField]
    private int _score;

    // variable for the shield visualiser
    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private GameObject[] _damage = new GameObject[2];
    [SerializeField]
    private GameObject _explosionPrefab;
    private UIManager _ui;
    // Start is called before the first frame update
    AudioSource audioSource;
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _ui = GameObject.FindObjectOfType<UIManager>();
        _ui.UpdateLives(_lives);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            UnityEngine.Debug.LogError("Audio source on player is null!");
        }
        else
        {
            audioSource.clip = _LaserShotSound;
        }
        //take current position = new position (0,0,0)
        transform.position = new Vector3(0f, 0f, 0f); 
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        // spawn laser object
        if (Input.GetKeyDown(KeyCode.Space)&&Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        //find horizontal and vertical control values
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        // new Vector3(1,0,0) * _speed * real time
        if (_speedActive == true)
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.4f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.4f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_trippleShotActive == false)
        {
            Vector3 offset = new Vector3(0, 1.05f, 0);
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        }
        else
        {
            Instantiate(_trippleShotPrefab, transform.position, Quaternion.identity);
        }

        audioSource.clip = _LaserShotSound;
        audioSource.Play();
    }

    public void Damage()
    {
        if (!_shieldActive)
        {
            _lives--;
            if (_lives == 2)
            {
                _damage[UnityEngine.Random.Range(0, 2)].SetActive(true);
            }
            else if(_lives ==1)
            {
                foreach(GameObject go in _damage)
                {
                    go.SetActive(true);
                }
            }
            _ui.UpdateLives(_lives);
            if (_lives <= 0)
            {

                _spawnManager.OnPlayerDeath();
                Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
                _ui.GameOver();
                Destroy(this.gameObject);
            }
        }
        _shieldActive = false;
        _shield.SetActive(false);

    }
    public void Powerup(string gameObjectTag)
    {
        switch(gameObjectTag)
        {
            case "Powerup_Tripple":                
                    _trippleShotActive = true;
                    StartCoroutine(TrippleShotPowerDown());                
                break;
            case "Powerup_Speed":                
                    _speedActive = true;
                    StartCoroutine(SpeedPowerDown());                
                break;
            case "Powerup_Shield":
                _shieldActive = true;
                _shield.SetActive(true);
   
                break;
            default:    
                break;
        }
        audioSource.clip = _pickupSound;
        audioSource.Play();
    }

    private IEnumerator TrippleShotPowerDown()
    {
        _trippleShotDeactivate = Time.time + _trippleShotActiveTime;
        yield return new WaitUntil(() => (Time.time > _trippleShotDeactivate));
        _trippleShotActive = false;
    }

    private IEnumerator SpeedPowerDown()
    {
        _speedDeactivate = Time.time + _speedActiveTime;
        yield return new WaitUntil(() => (Time.time > _speedDeactivate));
        _speedActive = false;
    }

    public void AddScore(int score)
    {
        _score += score;

        _ui.UpdateScore(_score);
    }
}
