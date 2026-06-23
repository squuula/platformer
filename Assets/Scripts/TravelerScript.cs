using UnityEngine;
using System.Collections;

public class LevelExitNPC : MonoBehaviour
{
    public Animator victoryMenuAnimator;

    public float npcAnimationDuration = 1.5f;

    private Animator npcAnim;
    private bool levelFinished = false;

    void Start()
    {
        npcAnim = GetComponent<Animator>();
    }

    public int thisLevelNumber = 1; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !levelFinished)
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null && player.HasCollectedAllEmeralds())
            {
                StartCoroutine(FinishLevelRoutine(player));
            }
        }
    }

    IEnumerator FinishLevelRoutine(PlayerController player)
    {
        levelFinished = true;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideGameplayHUD();
        }

        if (player != null)
        {
            player.enabled = false;

            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(0f, playerRb.linearVelocity.y);
            }

            Animator playerAnim = player.GetComponent<Animator>();

            if (playerAnim != null)
            {
                playerAnim.SetFloat("speed", 0f);
            }
        }

        if (npcAnim != null)
        {
            npcAnim.SetTrigger("Success");
        }

        yield return new WaitForSeconds(npcAnimationDuration);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowFinalResults(thisLevelNumber);
        }

        if (victoryMenuAnimator != null)
        {
            victoryMenuAnimator.SetTrigger("ShowMenu");
        }

        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}