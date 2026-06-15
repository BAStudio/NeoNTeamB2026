using UnityEngine;

public class Lock : MonoBehaviour
{
    private int hitCount = 0;
    [SerializeField] private int hitsToDestroy = 3;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void TakeHit()
    {
        hitCount++;

        transform.localScale = originalScale * 1.2f;
        Invoke(nameof(ResetScale), 0.15f);

        if (hitCount >= hitsToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void ResetScale()
    {
        transform.localScale = originalScale;
    }
}
