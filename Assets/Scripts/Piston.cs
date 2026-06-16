using System.Collections;
using UnityEngine;

/// <summary>
/// Повесить на объект поршня.
/// Нужен Animator с триггером "Activate".
/// Коллайдер НЕ триггер.
/// </summary>
[RequireComponent(typeof(Animator))]
public class Piston : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Сила подброса игрока")]
    public float launchForce = 20f;

    [Tooltip("Задержка перед подбросом (совпадает с моментом анимации удара)")]
    public float launchDelay = 0.2f;

    Animator anim;
    bool active = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (active) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                StartCoroutine(Launch(collision.gameObject));
                return;
            }
        }
    }

    IEnumerator Launch(GameObject player)
    {
        active = true;
        anim.SetTrigger("Activate");

        yield return new WaitForSeconds(launchDelay);

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, launchForce);

        yield return new WaitForSeconds(0.5f);
        active = false;
    }
}
