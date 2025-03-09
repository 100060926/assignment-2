using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlowMotionPowerup : MonoBehaviour
{
    private float slowEffectDuration = 5f; // How long the slow effect lasts
    private float enemySlowFactor = 0.15f; // How much enemies slow down
    public Slider slowMotionSlider; // UI Slider to display effect duration
    public GameObject slowEffectIndicator; // Visual indicator for the player

    private bool isSlowEffectActive = false; // Prevent multiple activations

    public void ActivateSlowMotion()
    {
        if (!isSlowEffectActive)
        {
            StartCoroutine(ApplySlowMotionEffect());

            // Notify the spawn manager that the powerup was collected
            SpawnManagerGame1 spawnManager = FindObjectOfType<SpawnManagerGame1>();
            if (spawnManager != null)
            {
                spawnManager.PowerupCollected();
            }
        }
    }

    private IEnumerator ApplySlowMotionEffect()
    {
        isSlowEffectActive = true;
        Debug.Log("Slow Motion Powerup Activated!");

        // Show effect indicator on the player
        if (slowEffectIndicator != null)
        {
            slowEffectIndicator.SetActive(true);
        }

        // Show and initialize slider UI
        if (slowMotionSlider != null)
        {
            slowMotionSlider.gameObject.SetActive(true);
            slowMotionSlider.maxValue = slowEffectDuration;
            slowMotionSlider.value = slowEffectDuration; // Start full
        }

        float timeLeft = slowEffectDuration;

        // Slow down all enemies
        EnemyPlayerFollower[] playerFollowers = FindObjectsOfType<EnemyPlayerFollower>();
        EnemyGoalFollower[] goalSeekers = FindObjectsOfType<EnemyGoalFollower>();
        ShieldedEnemy[] shieldedEnemies = FindObjectsOfType<ShieldedEnemy>();
        SpeedBoosterEnemy[] speedBoosters = FindObjectsOfType<SpeedBoosterEnemy>();

        foreach (var enemy in playerFollowers) enemy.speed *= enemySlowFactor;
        foreach (var enemy in goalSeekers) enemy.speed *= enemySlowFactor;
        foreach (var enemy in shieldedEnemies) enemy.speed *= enemySlowFactor;
        foreach (var enemy in speedBoosters) enemy.speed *= enemySlowFactor;

        // Countdown UI
        while (timeLeft > 0)
        {
            if (slowMotionSlider != null)
            {
                slowMotionSlider.value = timeLeft; // Update slider value
            }
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        // Reset enemy speeds
        foreach (var enemy in playerFollowers) enemy.speed /= enemySlowFactor;
        foreach (var enemy in goalSeekers) enemy.speed /= enemySlowFactor;
        foreach (var enemy in shieldedEnemies) enemy.speed /= enemySlowFactor;
        foreach (var enemy in speedBoosters) enemy.speed /= enemySlowFactor;

        Debug.Log("Slow Motion Effect Ended!");

        // Hide UI and reset effect flag
        if (slowMotionSlider != null) slowMotionSlider.gameObject.SetActive(false);

        // Hide effect indicator
        if (slowEffectIndicator != null)
        {
            slowEffectIndicator.SetActive(false);
        }

        isSlowEffectActive = false;
    }
}
