using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string actionID;

        [TextArea(2, 5)]
        public string[] lines;
    }

    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public GameObject NextIndicator;

    [Header("Dialogue Database")]
    public DialogueEntry[] dialogues;

    [Header("Settings")]
    public float textSpeed = 0.05f;

    private string[] currentLines;
    private int index;

    void OnEnable()
    {
        GameEvents.OnDialogueStart += HandleAction;
    }

    void OnDisable()
    {
        GameEvents.OnDialogueStart -= HandleAction;
    }

    void Update()
    {

        if (currentLines == null || index < 0 || index >= currentLines.Length)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == currentLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = currentLines[index];
            }
        }
    }

    public void ShowNextIndicator()
    {
        if (NextIndicator == null || currentLines == null || dialogueText == null)
            return;

        NextIndicator.SetActive(dialogueText.text == currentLines[index] && index < currentLines.Length - 1);
    }

    void HandleAction(string actionID)
    {
        if (dialogueText == null)
        {
            Debug.LogError($"{nameof(DialogueManager)} is missing a reference to {nameof(dialogueText)}.", this);
            return;
        }

        StopAllCoroutines();
        dialogueText.text = string.Empty;
        NextIndicator?.SetActive(false);

        if (dialogues == null)
        {
            currentLines = null;
            return;
        }

        foreach (DialogueEntry dialogue in dialogues)
        {
            if (dialogue.actionID == actionID)
            {
                currentLines = dialogue.lines;
                index = 0;

                StartCoroutine(TypeLine());
                return;
            }
        }

        currentLines = null;
    }

    IEnumerator TypeLine()
    {
        dialogueText.text = string.Empty;
        NextIndicator?.SetActive(false);

        foreach (char c in currentLines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        ShowNextIndicator();
    }

    void NextLine()
    {
        if (index < currentLines.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            currentLines = null;
            dialogueText.text = "";
        }
    }
}