using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Transform player;
    [SerializeField] Transform cameraTransform;

    Vector3 playerStartPos;
    Vector3 cameraStartPos;
    Transform cameraOriginalParent;

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

        StoreInitialState();
    }

    void StoreInitialState()
    {
        playerStartPos = player.position;
        cameraStartPos = cameraTransform.localPosition;
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
