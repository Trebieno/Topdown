using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCache : MonoBehaviour
{
    public static PlayerCache Instance { get; private set; }

    // [SerializeField] private Dictionary<GameObject, Player> _players = new Dictionary<GameObject, Player>();
    public Player Player;
    // public Dictionary<GameObject, Player> Players => _players;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
