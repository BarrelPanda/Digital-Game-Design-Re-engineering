using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float _maxhealth = 20f;
    [SerializeField] private float _baseatk = 1f;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _animationTime = 0.8f;
    [SerializeField] private float _attackSpeed = 1.5f;
    public bool _tutorial;

    private float _currenthealth;
    private SpriteRenderer _spriteRenderer;
    private CharacterStats _playerStats;
    private HealthUI _healthBar;
    private Slider _attackspeedUI;

    private void Start()
    {
        _currenthealth = _maxhealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerStats = FindObjectOfType<CharacterStats>().GetComponent<CharacterStats>();
        StartCoroutine(Attack());
        _healthBar = GetComponent<HealthUI>();
        _attackspeedUI = GameObject.Find("attackspeed").GetComponent<Slider>();
        _healthBar.AdjustHealthUI(_currenthealth / _maxhealth);
    }

    public void TakeDmg(float dmgTaken)
    {
        _currenthealth -= dmgTaken;
        _healthBar.AdjustHealthUI(_currenthealth / _maxhealth);
        if (_currenthealth <= 0)
        {
            _playerStats.ClearBonus();
            FindObjectOfType<Manager>().CombatComplete(true);
            Destroy(gameObject);
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
        // 3 = ded
    }

    IEnumerator Attack()
    {
        while (!_tutorial)
        {
            float x = 0;
            float currentamt = 0;
            while (x < _attackSpeed)
            {
                yield return new WaitForSeconds(0.1f);
                x += 0.1f;
                currentamt += 0.1f;
                _attackspeedUI.value = currentamt/_attackSpeed;
            }
            _playerStats.TakeDmg(_baseatk);
        }
    }
}
