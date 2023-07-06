using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkLobbyExit : MonoBehaviour
{
    public void LobbyExit()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        
        else if (NetworkClient.isConnected)
            NetworkManager.singleton.StopClient();
        
        else if(NetworkServer.active)
            NetworkManager.singleton.StopServer();
    }   
}
