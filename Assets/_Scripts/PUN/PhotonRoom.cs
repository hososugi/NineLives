using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Room info
    public static PhotonRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    // Player info
    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;
    public int playerInGame;

    private void Awake()
    {
        // Set up a singleton.
        if(PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if(PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to the functions.
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe to the function.
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void Start()
    {
        // Initialization.
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("INFO: joined a room!");
        
        // Set player data.
        photonPlayers          = PhotonNetwork.PlayerList;
        playersInRoom          = photonPlayers.Length;
        myNumberInRoom         = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        // For starting the game.
        StartGame();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("INFO: a new player joined the room.");

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;


    }

    void StartGame()
    {
        isGameLoaded = true;

        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.gameScene);
        
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // Called when the scene is loaded.
        currentScene = scene.buildIndex;

        if(currentScene == MultiplayerSettings.multiplayerSettings.gameScene)
        {
            isGameLoaded = true;

            RPC_CreatePlayer();
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerController"), new Vector3(0, 0, 0), Quaternion.identity, 0);
    }
}
