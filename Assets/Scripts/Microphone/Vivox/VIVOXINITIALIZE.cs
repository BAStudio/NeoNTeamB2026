using UnityEngine;
using Unity.Services.Vivox;
using Unity.Services.Vivox.AudioTaps;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class Vivoxconnect : MonoBehaviour
{
    [SerializeField] private VivoxParticipantTap participantTap;
    [SerializeField] private string channelName = "testingchannels";
    [SerializeField] private VivoxCaptureSourceTap captureSourceTap;
    public string playerhostid;

    async void Start()
    {

        await UnityServices.InitializeAsync();
        Debug.Log("Vivox Initialized");

        Debug.Log("This is the local player");
        channelName = channelName + AuthenticationService.Instance.PlayerId;
        playerhostid = channelName;
        Debug.Log("Player Host ID: " + playerhostid);
        JoinChannelAsync(channelName);
        gameObject.GetComponent<VivoxParticipantTap>();
        //participantTap.VoiceTransmitEnabled = true;
        //participantTap.ParticipantName;
        participantTap.ChannelName = channelName;
        gameObject.GetComponent<VivoxCaptureSourceTap>();
        captureSourceTap.ChannelName = channelName;
    }



    void OnChannelJoined(string channelName)
    {

        Debug.Log("Joined channel: " + channelName);
        //gameObject.GetComponent<VivoxParticipantTap>();
        ////participantTap.VoiceTransmitEnabled = true;
        //participantTap.ParticipantName = AuthenticationService.Instance.PlayerId;
        //participantTap.ChannelName = channelName;
        //gameObject.GetComponent<VivoxCaptureSourceTap>();
        //captureSourceTap.ChannelName = channelName;
    }

    void OnChannelLeft(string channelName)
    {
        Debug.Log("Left channel: " + channelName);
    }

    async void JoinChannelAsync(string channelName)
    {
        await
        VivoxService.Instance.JoinGroupChannelAsync(channelName, ChatCapability.AudioOnly);

        Debug.Log("Vivox Joined Channel");
    }

}