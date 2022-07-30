using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public CombatController player;
    public CombatController enemy;

    public Image playerBar;
    public Image enemyBar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerBar.fillAmount = player.currentData.currentHealth.value / player.currentData.maxHealth.value;
        enemyBar.fillAmount = enemy.currentData.currentHealth.value / enemy.currentData.maxHealth.value;
    }
}
