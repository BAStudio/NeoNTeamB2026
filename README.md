# NeoNTeamB2026

## UI animation
```mermaid
classDiagram
    class UIAnimations {
        <<static>>
        +ScaleElement(MonoBehaviour host, RectTransform rect, Vector3 targetScale, float duration)
        +BookTransition(MonoBehaviour host, RectTransform rect, bool open, float duration)
        -ScaleCoroutine(RectTransform rect, Vector3 targetScale, float duration) IEnumerator
    }
```

## Dialogue Manager

```mermaid
classDiagram
    class DialogueManager {
        <<MonoBehaviour>>
        +TextMeshProUGUI dialogueText
        +GameObject NextIndicator
        +DialogueEntry[] dialogues
        +float textSpeed
        -string[] currentLines
        -int index
        -OnEnable()
        -OnDisable()
        -Update()
        +ShowNextIndicator()
        -HandleAction(string actionID)
        -TypeLine() IEnumerator
        -NextLine()
    }

    class DialogueEntry {
        +string actionID
        +string[] lines
    }

    DialogueManager *-- DialogueEntry : "bevat lijst van"

```

## Health popup

```mermaid 
classDiagram
    class ShowHealth {
        <<MonoBehaviour>>
        -KeyCode PopupKey
        -GameObject HealthWatch
        -float PopupSpeed
        -float PopupAmount
        -bool IsUP
        -Vector3 startLocalPosition
        -Vector3 targetLocalPosition
        -Start()
        -Update()
    }
```

## UI slots

```mermaid
graph TD
    A[Start: OnEnable] --> B{InventorySystem beschikbaar?}
    
    B -- Ja --> C[Subscribe aan OnInventoryChanged]
    C --> D[UpdateSlotVisuals]
    
    B -- Nee --> E[Start Coroutine: WaitForInventorySystem]
    E --> F{InventorySystem gevonden?}
    F -- Nee --> F
    F -- Ja --> G[Subscribe aan OnInventoryChanged]
    G --> D
    
    D --> H{Item in slot?}
    H -- Ja --> I[Toon item naam]
    H -- Nee --> J[Maak tekst leeg/onzichtbaar]
    
    I --> K{Slot is actief?}
    J --> K
    
    K -- Ja --> L[Kleur = ActiveColor]
    L --> M[Start Scale Animation: 1.2x]
    
    K -- Nee --> N[Kleur = InactiveColor]
    N --> O[Start Scale Animation: 1.0x]
    
    %% Cleanup
    P[OnDisable] --> Q[Unsubscribe]
    Q --> R[Stop Coroutine]

```
