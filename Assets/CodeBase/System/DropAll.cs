using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAll : MonoBehaviour
{
    public static DropAll Instance { get; private set; }

    [SerializeField] private Dictionary<GameObject, WeaponData> _drops = new Dictionary<GameObject, WeaponData>();
    public Dictionary<GameObject, WeaponData> Drops => _drops;
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
