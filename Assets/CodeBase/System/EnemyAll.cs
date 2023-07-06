using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAll : MonoBehaviour
{
    public static EnemyAll Instance { get; private set; }

    [SerializeField] private Dictionary<GameObject, Character> _enemies = new Dictionary<GameObject, Character>();
    public Dictionary<GameObject, Character> Enemies => _enemies;

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
