using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class InteractLift : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject textObject;
    [SerializeField] private TMP_Text notificationText;

    [Header("Settings")]
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private float textSpeed = 0.05f;

    [Header("Notifications")]
    [SerializeField] private NotificationData[] notifications;

    private string lastActiveTag = "";
    private Coroutine typingCoroutine;

    [Serializable]
    public class NotificationData
    {
        public string tagName;
        [TextArea(3, 10)]
        public string message;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out hit,
            raycastDistance))
        {
            bool foundNotification = false;

            foreach (NotificationData data in notifications)
            {
                if (hit.transform.CompareTag(data.tagName))
                {
                    foundNotification = true;

                    if (lastActiveTag != data.tagName)
                    {
                        lastActiveTag = data.tagName;
                        textObject.SetActive(true);

                        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                        typingCoroutine = StartCoroutine(TypeLine(data.message));
                    }
                    break;
                }
            }

            if (!foundNotification)
            {
                ClearNotification();
            }
        }
        else
        {
            ClearNotification();
        }
    }

    IEnumerator TypeLine(string message)
    {
        notificationText.text = string.Empty;
        foreach (char c in message)
        {
            notificationText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void ClearNotification()
    {
        if (string.IsNullOrEmpty(lastActiveTag) && !textObject.activeSelf)
            return;

        lastActiveTag = string.Empty;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        notificationText.text = string.Empty;
        textObject.SetActive(false);
    }
}