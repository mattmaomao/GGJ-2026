using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    Collider2D currentCollider;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            playerMovement.isGrounded = true;
            AudioManager.Instance.PlaySE(AudioManager.SE_LANDING);
            currentCollider = collision;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && collision == currentCollider)
        {
            playerMovement.isGrounded = false;
        }
    }
}
