using UnityEngine;
using System;
using UnityEngine.XR;
public class Player : MonoBehaviour
{
    private InputManager _inputManager;
    private Rigidbody2D _rb;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _moveSpeed;
    private void Start()
    {
        _inputManager = InputManager.instance;
        _inputManager.onSpacebarPressed += Spacebar;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        TurnShip();
        _rb.angularVelocity = _turnSpeed * _inputManager.TurnInput();
        _rb.linearVelocity = _moveSpeed * transform.up;
    }

    public void TurnShip()
    {

    }

    public void Spacebar()
    {
        
    }
}
