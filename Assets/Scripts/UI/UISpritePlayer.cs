using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 12f;
    public bool loop = true;

    private Image img;
    private int index;
    private float timer;
    private bool playing = true;

    void Awake()
    {
        img = GetComponent<Image>();
        
    }

    void OnEnable()
    {
        index = 0;
        timer = 0f;
        playing = true;

        if (frames != null && frames.Length > 0)
            img.sprite = frames[0];
    }

    void Update()
    {
        if (!playing) return;
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;
        float frameTime = 1f / Mathf.Max(1f, fps);

        while (timer >= frameTime)
        {
            timer -= frameTime;
            index++;

            if (index >= frames.Length)
            {
                if (loop) index = 0;
                else { index = frames.Length - 1; playing = false; }
            }

            img.sprite = frames[index];
        }
    }

    public void Play() => playing = true;
    public void Stop() => playing = false;

    public void PlayFromStart()
    {
        index = 0;
        timer = 0f;
        playing = true;
        if (frames != null && frames.Length > 0)
            img.sprite = frames[0];
    }
}