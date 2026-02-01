using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] ColorMaskable colorMaskable;
    [SerializeField] SpriteRenderer spriteRenderer;

    public void Init(Vector2 pos, Color color, PlatformColorType colorType)
    {
        if (colorType == PlatformColorType.Null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.transform.position = pos;
        spriteRenderer.color = color;
        colorMaskable.colorType = colorType;
    }
}
