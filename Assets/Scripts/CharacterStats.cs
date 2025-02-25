using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private float _maxhealth = 20f;
    [SerializeField] private float _baseatk = 1f;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _animationTime = 0.8f;

    private float _currenthealth;
    private HealthUI _healthBar;
    private float _atk;
    private SpriteRenderer _spriteRenderer;
    private EnemyScript _enemyScript;

    private void Start()
    {
        _currenthealth = _maxhealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _healthBar = GetComponent<HealthUI>();
        _healthBar.AdjustHealthUI(_currenthealth / _maxhealth);
    }

    public void SetEnemy()
    {
        _enemyScript = FindObjectOfType<EnemyScript>().GetComponent<EnemyScript>();
    }

    public void ClearBonus()
    {
        _maxhealth += 5;
        _baseatk += 1;
        _currenthealth = _maxhealth;
        _healthBar.AdjustHealthUI(_currenthealth / _maxhealth);
    }
     public void ResetGame()
    {
        _currenthealth = _maxhealth;
        _healthBar.AdjustHealthUI(_currenthealth / _maxhealth);
    }

    public void Attack(float dmgMult)
    {
        _atk = _baseatk * dmgMult;
        StartCoroutine(UpdateSprite(1));
        if (_enemyScript != null)
            _enemyScript.TakeDmg(_atk);
    }

    public void TakeDmg(float dmgTaken)
    {
        _currenthealth -= dmgTaken;
        _healthBar.AdjustHealthUI(_currenthealth / _maxhealth);
        if (_currenthealth <= 0)
        {
            Debug.Log("Player ded");
            FindObjectOfType<Manager>().CombatComplete(false);
            FindObjectOfType<EnemyScript>().StopAllCoroutines();
        }
        else
        {
            StartCoroutine(UpdateSprite(2));
        }
    }

    IEnumerator UpdateSprite(int costume)
    {
        _spriteRenderer.sprite = _sprites[costume];
        yield return new WaitForSeconds(_animationTime);
        _spriteRenderer.sprite = _sprites[0];

        // 0 = idle
        // 1 = attack
        // 2 = take dmg
    }
}