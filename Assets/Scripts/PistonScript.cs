using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float launchVelocity = 15f;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);

                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, launchVelocity);

                if (anim != null)
                {
                    anim.SetTrigger("jump");
                }
            }
        }
    }
}