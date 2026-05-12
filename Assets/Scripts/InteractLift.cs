using UnityEngine;
using TMPro;
using System;

public class InteractLift : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject textObject;
    [SerializeField] private TMP_Text notificationText;

    [Header("Notifications")]
    [SerializeField] private NotificationData[] notifications;

    [Serializable]
    public class NotificationData
    {
        public string tagName;
        public string message;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out hit,
            Mathf.Infinity))
        {
            bool foundNotification = false;

            foreach (NotificationData data in notifications)
            {
                if (hit.transform.CompareTag(data.tagName))
                {
                    textObject.SetActive(true);
                    notificationText.text = data.message;

                    foundNotification = true;
                    break;
                }
            }

            if (!foundNotification)
            {
                textObject.SetActive(false);
            }
        }
        else
        {
            textObject.SetActive(false);
        }
    }
}