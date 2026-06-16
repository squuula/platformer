using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Повесить на жителя (Traveler).
/// Нужен дочерний триггер-коллайдер зоны взаимодействия.
/// </summary>
public class VillagerInteraction : MonoBehaviour
{
    [Header("References")]
    public GameObject invisibleWall;
    public GameObject portal;

    [Header("Settings")]
    public float disappearDelay = 1f;

    Animator anim;
    bool playerNearby = false;
    bool activated = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (portal != null)
            portal.SetActive(false);
    }

    void Update()
    {
if (!playerNearby || activated) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
            TryInteract();
    }

    void TryInteract()
    {
        if (CollectibleManager.Instance == null || !CollectibleManager.Instance.AllCollected)
        {
            Debug.Log("Соберите все изумруды!");
            return;
        }

        activated = true;
        StartCoroutine(ActivateRoutine());
    }

    IEnumerator ActivateRoutine()
    {
        if (invisibleWall != null)
            invisibleWall.SetActive(false);

        if (anim != null)
            anim.SetTrigger("Death");

        yield return new WaitForSeconds(disappearDelay);

        if (portal != null)
            portal.SetActive(true);

        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}
