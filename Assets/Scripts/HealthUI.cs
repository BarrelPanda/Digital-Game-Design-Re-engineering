using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Slider _healthUI;

    [SerializeField] private bool _isPlayer;

    public void AdjustHealthUI(float _health)
    {
        if (_healthUI == null)
        {
            if (_isPlayer)
                _healthUI = GameObject.Find("PlayerHealth").GetComponent<Slider>();
            else
                _healthUI = GameObject.Find("EnemyHealth").GetComponent<Slider>();
        }
        _healthUI.value = _health;
    }
}
