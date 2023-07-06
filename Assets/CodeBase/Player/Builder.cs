using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Builder : NetworkBehaviour
{
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private Transform _buildPoint;
    [SerializeField] private Camera _camera;

    [SerializeField] private float _maxTimeBuild;
    private float _timeBuild;

    [SerializeField] private float _radiusDestroy;
    [SerializeField] private LayerMask _destroyLayer;
    [SerializeField] private float _maxTimeDestroy;
    private float _timeDestroy;
    
    private bool _keyBuild;
    private bool _keyDestroy;
    
    public event Action<float> OnConstructionStart;
    public event Action OnConstructionStop;

    public event Action<float> OnDestructionStart;
    public event Action OnDestructionStop;
    
    private void Start()
    {
        if(!isLocalPlayer) this.enabled = false;
        _timeBuild = _maxTimeBuild;
    }

    private Collider2D _previousCollider2D;
    public void Update()
    {
        Building();
        Destroing();
    }
    
    
    [Command]
    private void CmdSpawnObject(Vector3 position)
    {
        GridObjectPlacement.Instance.Items.RemoveAll(x => x == null);
        if (GridObjectPlacement.Instance.Items.Find(i => i.transform.position == position) == null)
        {
            GameObject newObject = Instantiate(_objectPrefab, position, Quaternion.identity);
            NetworkServer.Spawn(newObject);
            GridObjectPlacement.Instance.AddItem(newObject);
        }
    }
    
    [Command]
    private void CmdDestroyObject(GameObject gameObject)
    {
        NetworkServer.Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_buildPoint.position, _radiusDestroy);
    }

    private void Building()
    {
        if (Input.GetButtonDown("Build"))
        {
            _keyBuild = true;
        }


        if (Input.GetButtonUp("Build"))
        {
            _timeBuild = _maxTimeBuild;
            _timeDestroy = _maxTimeDestroy;
            _keyBuild = false;
            OnConstructionStop?.Invoke();
        }
        
        if (_keyBuild)
        {
            if (_timeBuild <= 0)
            {
                // var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                var cellPosition = GridObjectPlacement.Instance.Grid.WorldToCell(_buildPoint.position);
                cellPosition.z = 0;

                var playerCellPosition = GridObjectPlacement.Instance.Grid.WorldToCell(transform.position);
                playerCellPosition.z = 0;
                
                
                var worldPosition = GridObjectPlacement.Instance.Grid.GetCellCenterWorld(cellPosition);

                if (cellPosition != playerCellPosition)
                    CmdSpawnObject(worldPosition);
                _timeBuild = _maxTimeBuild;
            }
            else
            {
                _timeBuild -= Time.deltaTime;
                OnConstructionStart?.Invoke((_timeBuild/_maxTimeBuild)*100);
            }
        }
    }

    private void Destroing()
    {
        if (Input.GetButtonDown("Destroy"))
            _keyDestroy = true;


        if (Input.GetButtonUp("Destroy"))
        {
            _timeDestroy = _maxTimeDestroy;
            _keyDestroy = false;
            OnDestructionStop?.Invoke();
        }
    
        
        if (_keyDestroy)
        {
            var collider = Physics2D.OverlapCircle(_buildPoint.position, _radiusDestroy, _destroyLayer);
            if (_previousCollider2D != collider)
            {
                _timeDestroy = _maxTimeDestroy;
                OnDestructionStop?.Invoke();
            }

            if (collider != null)
            {
                if (_timeDestroy <= 0)
                {
                    CmdDestroyObject(collider.gameObject);
                    _timeDestroy = _maxTimeDestroy;
                    OnDestructionStop?.Invoke();
                }
                else
                {
                    _timeDestroy -= Time.deltaTime;
                    OnDestructionStart?.Invoke((_timeDestroy/_maxTimeDestroy)*100);
                }
            }

            _previousCollider2D = collider;
        }
    }
}

