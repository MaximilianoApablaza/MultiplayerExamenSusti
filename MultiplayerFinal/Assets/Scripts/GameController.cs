using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    public static GameController instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Debug.Log($"{PhotonNetwork.IsConnectedAndReady}" + $"{Player.LocalInstance}");
        if (PhotonNetwork.IsConnectedAndReady && Player.LocalInstance == null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 10, 0), Quaternion.identity);
            Debug.Log("Iniciado");
        }
    }

    public override void OnJoinedRoom()
    {
        if (Player.LocalInstance == null)
        {
            Debug.Log("a");
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 10, 0), Quaternion.identity);
        }
    }

}
