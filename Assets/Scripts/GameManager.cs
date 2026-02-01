using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform player;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Sprite smokePuffSprite;
    [SerializeField] float respawnDelay = 0.5f;

    Vector3 playerStartPos;
    Vector3 cameraStartPos;
    Transform cameraOriginalParent;
    SpriteRenderer playerSprite;
    Rigidbody2D playerRb;

    public bool IsDead { get; private set; } = false;

    public int currentLevel = 1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        cameraOriginalParent = cameraTransform.parent;
        playerSprite = player.GetComponent<SpriteRenderer>();
        playerRb = player.GetComponent<Rigidbody2D>();

        MapLoader.Instance.InitLevel(currentLevel);
    }

    public void StoreInitialState(Vector2 pos)
    {
        playerStartPos = pos;
        // cameraStartPos = cameraTransform.localPosition;
    }

    public void OnPlayerDeath()
    {
        if (IsDead) return;
        IsDead = true;

        // Freeze player immediately
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.simulated = false;
        }

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // Spawn smoke at player position
        if (smokePuffSprite != null)
        {
            GameObject smoke = new GameObject("SmokePuff");
            smoke.transform.position = player.position;
            SpriteRenderer sr = smoke.AddComponent<SpriteRenderer>();
            sr.sprite = smokePuffSprite;
            sr.sortingOrder = 100;
            Destroy(smoke, respawnDelay);
        }

        // Hide player sprite
        if (playerSprite != null)
            playerSprite.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // Reset and show player
        ResetToInitialState();
        if (playerSprite != null)
            playerSprite.enabled = true;

        // Re-enable physics
        if (playerRb != null)
            playerRb.simulated = true;

        // Reattach camera
        CameraDetach camDetach = cameraTransform.GetComponent<CameraDetach>();
        if (camDetach != null)
            camDetach.AttachCameraToPlayer();

        IsDead = false;
    }

    public void ResetToInitialState()
    {
        // Reset player
        player.position = playerStartPos;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Reset camera
        cameraTransform.parent = cameraOriginalParent;
        cameraTransform.localPosition = cameraStartPos;
    }
}
