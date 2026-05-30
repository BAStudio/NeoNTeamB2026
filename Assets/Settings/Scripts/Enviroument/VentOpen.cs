using UnityEngine;

public class VentOpen : MonoBehaviour
{
    public float openHeight = 2f;
    public float speed = 4f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isMoving = false;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * openHeight;
    }

    public void Open()
    {
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speed * Time.deltaTime
            );
        }
    }
}