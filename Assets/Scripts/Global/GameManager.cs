
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform Player { get; private set; }
    [SerializeField] private string playerTag = "Player";
    private HealthSystem playerHealthSystem;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private int currentWaveIndex = 0;
    private int currentSpawnCount = 0;
    private int waveSpawnCount = 0;
    private int waveSpawnPosCount = 0;

    public float spawnInterval = .5f;
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [SerializeField] private Transform spawnPositionsRoot;
    private List<Transform> spawnPositions = new List<Transform>();

    public List<GameObject> rewards = new List<GameObject>();

    [SerializeField] private CharacterStats defaultStats;
    [SerializeField] private CharacterStats rangedStats;

    private void Awake()
    {
        instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform;

        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;

        gameOverUI.SetActive(false);

        for(int i=0; i<spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
    }
    private void Start()
    {
        UpgradeStatInit();
        StartCoroutine("StartNextWave");
    }

    IEnumerator StartNextWave()
    {
        while(true)
        {
            if(currentSpawnCount ==0)
            {
                UpdateWaveUI();
                yield return new WaitForSeconds(2f);

                if(currentWaveIndex % 15 == 0)
                {
                    RandomUpgrade();
                }

                if(currentWaveIndex %10 == 0)
                {
                    waveSpawnPosCount = waveSpawnPosCount + 1 > spawnPositions.Count? waveSpawnCount : waveSpawnCount+1;
                    waveSpawnCount = 1;
                }
                if(currentWaveIndex % 5 == 0)
                {
                    CreateReward();
                }
                if(currentWaveIndex%3==0)
                {
                    waveSpawnCount += 1;
                }
                
                for(int i=0; i<waveSpawnPosCount; i++)
                {
                    int posIdx = Random.Range(0, spawnPositions.Count);
                    for(int j=0; j<waveSpawnCount; j++)
                    {
                        int prefabIdx = Random.Range(0, enemyPrefabs.Count);
                        GameObject enemy = Instantiate(enemyPrefabs[prefabIdx], spawnPositions[posIdx].position, Quaternion.identity);
                        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(defaultStats);
                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(rangedStats);
                        currentSpawnCount++;
                        yield return new WaitForSeconds(spawnInterval);
                    }
                }
                currentWaveIndex++;
            }
            yield return null;
        }
    }
    private void OnEnemyDeath()
    {
        currentSpawnCount--;
    }
    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth/playerHealthSystem.MaxHealth;
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        StopAllCoroutines();
        Time.timeScale = 0f;
    }
    private void UpdateWaveUI()
    {
        waveText.text =(currentWaveIndex+1).ToString();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("StartScene"); 
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    private void CreateReward()
    {
        int idx = Random.Range(0, rewards.Count);
        int posIdx = Random.Range(0, spawnPositions.Count);

        GameObject obj = rewards[idx];
        Instantiate(obj, spawnPositions[posIdx].position, Quaternion.identity);
    }

    private void UpgradeStatInit()
    {
        defaultStats.statsChangeType = StatsChangeType.Add;
        defaultStats.attackSO = Instantiate(defaultStats.attackSO);

        rangedStats.statsChangeType = StatsChangeType.Add;
        rangedStats.attackSO=Instantiate(rangedStats.attackSO);
    }

    private void RandomUpgrade()
    {
        switch(Random.Range(0,6))
        {
            case 0:
                defaultStats.maxHealth += 2;
                break;

            case 1:
                defaultStats.attackSO.power += 1;
                break;
            case 2:
                defaultStats.speed += 0.1f;
                break;
            case 3:
                defaultStats.attackSO.isOnKnockback = true;
                defaultStats.attackSO.knockbackPower += 1;
                defaultStats.attackSO.knockbackTime = 0.1f;
                break;
            case 4:
                defaultStats.attackSO.delay -= 0.05f;
                break;
            case 5:
                RangedAttackData rangedAttackData = rangedStats.attackSO as RangedAttackData;
                rangedAttackData.numberofProjectilesPershot += 1;
                break;

        }
    }
}
