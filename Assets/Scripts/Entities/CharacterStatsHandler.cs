using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    [SerializeField] private CharacterStats baseStats;
    public CharacterStats CurrentStats { get; private set; }
    public List<CharacterStats> statsModifiers = new List<CharacterStats>();

    private void Awake()
    {
        UpdateCharacterStats();
    }
    private void UpdateCharacterStats()
    {
        AttackSO attackSO = null;
        if(baseStats.attackSO != null)
        {
            attackSO = Instantiate(baseStats.attackSO); //attackSO를 하나 더 생성해서 자유롭게 변경하기 위해 안 하면 attackSO값이 바뀜
        }
        CurrentStats = new CharacterStats { attackSO = attackSO };
        CurrentStats.statsChangeType = baseStats.statsChangeType;
        CurrentStats.maxHealth = baseStats.maxHealth;
        CurrentStats.speed = baseStats.speed;
    }
}
