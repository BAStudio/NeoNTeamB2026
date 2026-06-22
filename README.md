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
classDiagram
    class MonoBehaviour {
        <<class>>
    }

    class Image {
        <<class>>
    }

    class TextMeshProUGUI {
        <<class>>
    }

    class Vector3 {
        <<struct>>
    }

    class Color {
        <<struct>>
    }

    class GameObject {
        <<class>>
    }

    class IEnumerator {
        <<interface>>
    }

    class InventorySystem {
        <<class>>
        +Instance : InventorySystem
        +OnInventoryChanged : Action
        +GetItemInSlot(slotIndex : int) : GameObject
        +GetCurrentSlotIndex() : int
    }

    class UIAnimations {
        <<class>>
        +ScaleElement(owner : MonoBehaviour, element : RectTransform, targetScale : Vector3, duration : float) : void
    }

    class RectTransform {
        <<class>>
    }

    class UISlot {
        -animationDuration : float = 0.15f
        -hoveredScale : Vector3
        -backgroundVisual : Image
        -slotIndex : int
        -nameText : TextMeshProUGUI
        -activeColor : Color
        -inactiveColor : Color
        -OnEnable() : void
        -OnDisable() : void
        -WaitForInventorySystem() : IEnumerator
        -UpdateSlotVisuals() : void
        -GetComponent~T~() : T
        -StartCoroutine(routine : IEnumerator) : void
        -StopCoroutine(routine : IEnumerator) : void
    }

    %% Overerving
    MonoBehaviour <|-- UISlot
    MonoBehaviour <|-- InventorySystem

    %% Associaties (Velden en Gebruik)
    UISlot --> Vector3 : hoveredScale
    UISlot --> Image : backgroundVisual
    UISlot --> TextMeshProUGUI : nameText
    UISlot --> Color : activeColor
    UISlot --> Color : inactiveColor
    UISlot --> InventorySystem : Gebruikt via Instance

    %% Interacties (binnen methoden)
    UISlot ..> GameObject : Verkrijgt item data
    UISlot ..> IEnumerator : Start/Stopt Coroutine
    UISlot ..> UIAnimations : Roept ScaleElement aan
    UISlot ..> RectTransform : Verkrijgt via GetComponent

```
