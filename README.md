# NeoNTeamB2026

classDiagram
    class UIAnimations {
        <<static>>
        +ScaleElement(MonoBehaviour host, RectTransform rect, Vector3 targetScale, float duration)
        +BookTransition(MonoBehaviour host, RectTransform rect, bool open, float duration)
        -ScaleCoroutine(RectTransform rect, Vector3 targetScale, float duration) IEnumerator
    }
