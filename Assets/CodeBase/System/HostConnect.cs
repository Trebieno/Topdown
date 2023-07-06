using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;


public class HostConnect : MonoBehaviour
{
    [SerializeField] private NetworkManager manager;
    [SerializeField] private TMP_InputField ip_InputField;
    [SerializeField] private GameObject HostConnect_go;
    
    public void HostFunction(){
        manager.StartHost();
        HostConnect_go.SetActive(false);
    }

    public void ConnectFunction(){
        manager.networkAddress = ip_InputField.text;
        manager.StartClient();
        // HostConnect_go.SetActive(false);
    }
}
