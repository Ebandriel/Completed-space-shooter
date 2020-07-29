using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _gameOverText.enabled = false;
        _restartText.enabled = false;
        _scoreText.text = "Score: " + 0;
        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int lives)
    {
        _livesImg.sprite = _livesSprite[lives];
    }

    public void GameOver()
    {

        _gameOverText.enabled = true;
        _restartText.enabled = true;
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerRoutine());

    }

    private IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);

        }
    }

    
}
