using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onDeath; // подписываем GameManager в Inspector

    bool isDead = false;

    // Вызывается стрелой/пулей или триггером ямы
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Можно добавить анимацию / звук здесь
        Debug.Log("Player died!");

        onDeath.Invoke(); // → GameManager.RestartLevel()
    }

    // Триггер ямы — объект с тегом "DeathZone" и Is Trigger = true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathZone"))
            Die();
    }
}
