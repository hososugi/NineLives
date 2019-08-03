using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myPlayer;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        int spawnPicker = 0;

        if(PV.IsMine)
        {
            Vector3 spawnLocation = GameSetup.GS.spawnPoints[spawnPicker].position;
            Quaternion spawnRotation = GameSetup.GS.spawnPoints[spawnPicker].rotation;

            myPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerAvatar"), spawnLocation, spawnRotation, 0);

            myPlayer.GetComponent<SimpleCharacterControl>().enabled = true;
            Camera.main.GetComponent<CameraFollow>().target = myPlayer.transform;
        }
    }
}
