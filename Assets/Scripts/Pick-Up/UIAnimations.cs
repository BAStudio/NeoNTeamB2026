using System.Collections;
using UnityEngine;

public static class UIAnimations
{
    public static void ScaleElement(MonoBehaviour host, RectTransform rect, Vector3 targetScale, float duration)
    {
        if (rect == null) return;
        host.StartCoroutine(ScaleCoroutine(rect, targetScale, duration));
    }

  
    public static void BookTransition(MonoBehaviour host, RectTransform rect, bool open, float duration)
    {
        if (rect == null) return;
        // If opening, target width scale is 1. If closing, it's 0.
        Vector3 targetScale = new Vector3(open ? 1f : 0f, rect.localScale.y, rect.localScale.z);
        host.StartCoroutine(ScaleCoroutine(rect, targetScale, duration));
    }


    private static IEnumerator ScaleCoroutine(RectTransform rect, Vector3 targetScale, float duration)
    {
        Vector3 startScale = rect.localScale;
        float time = 0;

        while (time < duration)
        {
            // USE unscaledDeltaTime so animations still play when Time.timeScale = 0
            time += Time.unscaledDeltaTime;

            float t = Mathf.SmoothStep(0f, 1f, time / duration);

            if (rect != null)
            {
                rect.localScale = Vector3.Lerp(startScale, targetScale, t);
            }
            yield return null;
        }

        if (rect != null)
        {
            rect.localScale = targetScale;
        }
    }
}