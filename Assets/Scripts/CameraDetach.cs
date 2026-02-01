using UnityEngine;

public class CameraDetach : MonoBehaviour
{
    [SerializeField] float minCameraPositionY = 0f;
    [SerializeField] float minPlayerY = -10f;

    [SerializeField] bool isDetached = false;
    public bool IsDetached => isDetached;

    [SerializeField] Transform player;
    Vector3 localPosWhenAttached;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        localPosWhenAttached = transform.localPosition;

        // Auto attach camera to player
        AttachCamera();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Detach when camera Y goes below minimum
        if (!isDetached && transform.position.y < minCameraPositionY)
        {
            DetachCamera();
        }
        // Re-attach when camera would be above minimum
        else if (isDetached && player.position.y + localPosWhenAttached.y >= minCameraPositionY)
        {
            AttachCamera();
        }

        // Death when player falls below minPlayerY
        if (player.position.y < minPlayerY)
        {
            OnPlayerDeath();
        }
    }

    void AttachCamera()
    {
        transform.parent = player;
        transform.localPosition = localPosWhenAttached;
        isDetached = false;
    }

    void DetachCamera()
    {
        transform.parent = null;
        isDetached = true;
    }

    void OnPlayerDeath()
    {
        Debug.Log("Player fell off screen!");
        AudioManager.Instance.PlaySE(AudioManager.SE_FAIL);

        GameManager.Instance.ResetToInitialState();
        AttachCamera();
    }
}
