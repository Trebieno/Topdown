using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : NetworkBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f; // Скорость поворота персонажа
    [SerializeField] private float _movementSpeed = 5f; // Скорость передвижения персонажа
    [SerializeField] private string _horizontalAxis = "Horizontal"; // Имя оси горизонтального движения
    [SerializeField] private string _verticalAxis = "Vertical"; // Имя оси вертикального движения
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [SerializeField] private Camera _camera;
    private void Start()
    {
        if (!isLocalPlayer)
        {
            _camera.gameObject.SetActive(false);
            this.enabled = false;
        }
        else
        {
            gameObject.name = "Client (Player)";
        }
    }
    
    public void FixedUpdate()
    {
        // Управление поворотом персонажа
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        // Управление передвижением персонажа
        float moveHorizontal = Input.GetAxis(_horizontalAxis);
        float moveVertical = Input.GetAxis(_verticalAxis);
        Vector3 movement = new Vector2(moveHorizontal, moveVertical) * _movementSpeed;
        _rigidbody2D.velocity = movement;
    }
}
