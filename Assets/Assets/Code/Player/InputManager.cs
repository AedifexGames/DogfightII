using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonBehaviour<InputManager>
{
    private float _turnInput = 0f;
    private bool _spacebarPressed = false;
    private PlayerInput _controls;
    private event Action onSpacebarPressed;
    private event Action onSpacebarRelease;
    private void InitalizePlayerInput()
    {
        _controls = new PlayerInput();
        _controls.Enable();
        _controls.Gameplay.TurnShip.performed += ctx => SetTurnInput(ctx.ReadValue<float>());
        _controls.Gameplay.Spacebar.started   += ctx => onSpacebarPressed?.Invoke();
        _controls.Gameplay.Spacebar.canceled  += ctx => onSpacebarRelease?.Invoke();
    }

    private void SetTurnInput(float input) { _turnInput = input; }
    public float TurnInput() { return _turnInput; }
    private void SetSpacebar(bool v) { _spacebarPressed = v; }
    public bool SpacebarPressed() { return _spacebarPressed; }
    
    private void Awake()
    {
        InitalizePlayerInput();
        onSpacebarPressed += () => SetSpacebar(true);
        onSpacebarRelease += () => SetSpacebar(false);
    }

    private void OnEnable()
    {
        _controls.Enable();
    }
    private void OnDisable()
    {
        _controls.Disable();
    }
}