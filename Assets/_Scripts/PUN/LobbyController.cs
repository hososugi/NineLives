using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    public static LobbyController lobbyController;
    public GameObject playButton;

    private void Awake()
    {
        lobbyController = this;
        Screen.fullScreen = !Screen.fullScreen;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("INFO: Player connected to Photon master server.");
        PhotonNetwork.AutomaticallySyncScene = true;

        playButton.SetActive(true);
        playButton.GetComponent<Button>().interactable = true;
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("INFO: Clicked the play button.");

        playButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom(null, 0);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("ERROR: OnJoinRandom failed.");

        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("INFO: trying to create a room.");

        string randomRoomName = "Room" + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible  = true,
            IsOpen     = true,
            MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers
        };

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions, null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("ERROR: Creating a room failed.");
        base.OnCreateRoomFailed(returnCode, message);

        CreateRoom();
    }
}
