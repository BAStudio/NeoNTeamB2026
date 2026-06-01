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
        if (currentLines == null)
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
        if (dialogueText.text == currentLines[index])
        {
            NextIndicator.SetActive(true);
        }
    }

    void HandleAction(string actionID)
    {
        foreach (DialogueEntry dialogue in dialogues)
        {
            if (dialogue.actionID == actionID)
            {
                currentLines = dialogue.lines;
                index = 0;

                dialogueText.text = "";
                StartCoroutine(TypeLine());

                return;
            }
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in currentLines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
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