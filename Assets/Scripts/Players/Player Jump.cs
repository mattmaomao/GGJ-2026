using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            playerMovement.isGrounded = true;
        }
    }
}
