using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    EnemySpawner enemySpawner;
    PlayerController playerController;
    public static GameManager instance;
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverText;
    public Button resetOrContinueButton;
    public Button menuButton;
    public TextMeshProUGUI timerText, scoreText, setTimerText, setSpawnRateText;
    public TMP_InputField spawnRateInput, roundTimeInput;
    public float timer;
    public float spawnRate;
    [SerializeField]
    float score;
    bool startTimer;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        enemySpawner = FindObjectOfType<EnemySpawner>();
        playerController = FindObjectOfType<PlayerController>();
        menuButton.onClick.AddListener(MainMenu);
    }

    private void Start()
    {
        PlayerController.instance.enabled = false;
        ///PlayGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            timerText.text = niceTime;
            if (timer <= 0)
            {
                timer = 0;
                GameWin();
            }
        }
    }

    public void PlayGame()
    {
        //Time.timeScale = 1;
        timer = PlayerPrefs.GetFloat("Timer", 60f);
        spawnRate = PlayerPrefs.GetFloat("SpawnRate", 5f);
        EnemySpawner.instance.StartSpawn();
        PlayerController.instance.enabled = true;
        PlayerController.instance.gameObject.transform.position = Vector2.zero;
        PlayerController.instance.GetComponent<HealthManager>().ResetHealth();
        startTimer = true;
    }

    public void ResetGame()
    {
        Debug.Log("GameReseting");
        //Time.timeScale = 1;
        EnemySpawner.instance.Reset();
        PlayerController.instance.GetComponent<HealthManager>().ResetHealth();
        PlayerController.instance.GetComponentInChildren<DeteriorationAnimationController>().Reset();
        PlayerController.instance.gameObject.transform.position = Vector2.zero;
        gameOverScreen.SetActive(false);
        spawnRate = PlayerPrefs.GetFloat("SpawnRate", 1.5f);
        ResetTimer();
        PlayerController.instance.enabled = true;
    }
    public void ContinueGame()
    {
        Debug.Log("GameContinue");
        //Time.timeScale = 1;
        EnemySpawner.instance.Reset();
        PlayerController.instance.GetComponent<HealthManager>().ResetHealth();
        gameOverScreen.SetActive(false);
        spawnRate = PlayerPrefs.GetFloat("SpawnRate", 1.5f);
        ResetTimer();
        PlayerController.instance.enabled = true;
    }
    public void GameOver()
    {
        DestroyAllCannonBals();
        PlayerController.instance.enabled = false;
        EnemySpawner.instance.StopEnemies();
        gameOverScreen.SetActive(true);
        gameOverText.text = "You died";
        scoreText.text = "Score: " + score;
        resetOrContinueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart Game";
        resetOrContinueButton.onClick.RemoveAllListeners();
        resetOrContinueButton.onClick.AddListener(ResetGame);
        //Time.timeScale = 0;
    }

    public void GameWin()
    {
        DestroyAllCannonBals();
        PlayerController.instance.enabled = false;
        EnemySpawner.instance.StopEnemies();
        //EnemySpawner.instance.StopAllCoroutines();
        gameOverScreen.SetActive(true);
        gameOverText.text = "Round Over";
        scoreText.text = "Score: " + score;
        resetOrContinueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Round";
        resetOrContinueButton.onClick.RemoveAllListeners();
        resetOrContinueButton.onClick.AddListener(ContinueGame);
        //PlayerController.instance.enabled = false;
        //Time.timeScale = 0;
    }

    void ResetTimer()
    {
        timer = PlayerPrefs.GetFloat("Timer", 60f);
        startTimer = true;
    }

    public void MainMenu()
    {
        //Time.timeScale = 1;
        EnemySpawner.instance.ResetToMenu();
        PlayerController.instance.GetComponent<HealthManager>().ResetHealth();
        PlayerController.instance.GetComponentInChildren<DeteriorationAnimationController>().Reset();
        PlayerController.instance.gameObject.transform.position = Vector2.zero;
    }

    public void DestroyAllCannonBals()
    {
        ProjectileBehavior[] projectile = FindObjectsOfType<ProjectileBehavior>();

        if (projectile.Length > 0)
        {
            foreach (ProjectileBehavior i in projectile)
            {
                Destroy(i);
            }
        }
    }

    public void SetSpawnRate()
    {
        if (spawnRateInput.text != "")
        {
            float value = 0;

            if (float.TryParse(spawnRateInput.text, out value))
            {
                Debug.Log(value);
                ///float value = float.tryParse(spawnRateInput.text);
                if (value > -1)
                {
                    PlayerPrefs.SetFloat("SpawnRate", value);
                    ConfigurationScreen();
                }
                else
                    return;
            }
        }
        else
            return;
    }

    public void SetRoundTime()
    {
        if (roundTimeInput.text != "")
        {
            float value = 0;
            if (float.TryParse(roundTimeInput.text, out value))
            {
                Debug.Log(value);
                ///float value = float.tryParse(spawnRateInput.text);
                if (value >= 5f)
                {
                    PlayerPrefs.SetFloat("Timer", value);
                    ConfigurationScreen();
                }
                else
                    return;
            }
        }
        else
            return;
    }

    public void ConfigurationScreen()
    {
        setSpawnRateText.text = "Enemy Spawn Rate : " + PlayerPrefs.GetFloat("SpawnRate", 5f) + "s";
        setTimerText.text = "Round Timer: " + PlayerPrefs.GetFloat("Timer", 60f) + "s";
    }

    public void AddScore(float value)
    {
        score += value;
        scoreText.text = "Score: " + score;
    }

}
