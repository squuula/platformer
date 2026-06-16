using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onDeath;

    [Header("Settings")]
    public float deathDelay = 1.5f;

    bool isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died!");
        GetComponent<PlayerController>()?.PlayDeath();
        FindObjectOfType<HUD>()?.AddDeath();
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        onDeath.Invoke();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathZone"))
            Die();
    }
}