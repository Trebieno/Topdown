using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GridObjectPlacement : NetworkBehaviour
{
    public static GridObjectPlacement Instance { get; private set; } 
    [SerializeField] private Grid _grid;
    
    [SerializeField] private SyncList<GameObject> _items = new SyncList<GameObject>();

    public SyncList<GameObject> Items => _items;
    public Grid Grid => _grid;

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

    public void AddItem(GameObject gameObject)
    {
        _items.RemoveAll(x => x == null);
        _items.Add(gameObject);
    }
}
