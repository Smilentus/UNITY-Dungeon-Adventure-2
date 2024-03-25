using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class CinematicsKeyboardInputHandler : MonoBehaviour, ICinematicsInputHandler
{
    public event Action onForceSkip;
    public event Action<bool> onToggled;

    public event Action<float> onProgress;
    public event Action<bool> onPressed;


    [SerializeField]
    private float m_passTime = 3f;

    [SerializeField]
    private InputActionReference m_skipAction;


    private float _pressTimer;
    private bool _isPressing;


    private bool _isEnabled;


    private void Awake()
    {
        if (m_skipAction == null)
        {
            Debug.LogError($"[CinematicsKeyboardInputHandler] m_skipAction not set!");
            return;
        }

        _isEnabled = false;

        // Вообще не здесь будет
        m_skipAction.action.Enable();

        m_skipAction.action.started += OnActionStarted;
        m_skipAction.action.canceled += OnActionCanceled;
    }

    private void OnDestroy()
    {
        m_skipAction.action.started -= OnActionStarted;
        m_skipAction.action.canceled -= OnActionCanceled;
    }


    private void Update()
    {
        if (!_isEnabled) return;

        if (_isPressing)
        {
            _pressTimer += Time.deltaTime;

            if (_pressTimer >= m_passTime)
            {
                _isPressing = false;
                onPressed?.Invoke(false);

                _pressTimer = m_passTime;

                onForceSkip?.Invoke();
            }
        }
        else
        {
            //if (_pressTimer > 0)
            //{
            //    _pressTimer -= Time.deltaTime;
            //}
            //else
            //{
            //    _pressTimer = 0;
            //}

            _pressTimer = 0;
        }

        _pressTimer = Mathf.Clamp(_pressTimer, 0, m_passTime);

        onProgress?.Invoke((_pressTimer / m_passTime));
    }


    private void OnActionStarted(InputAction.CallbackContext obj)
    {
        if (!_isEnabled) return;

        _isPressing = true;
        onPressed?.Invoke(true);
    }

    private void OnActionCanceled(InputAction.CallbackContext obj)
    {
        if (!_isEnabled) return;

        _isPressing = false;
        onPressed?.Invoke(false);
    }


    public void Toggle(bool status)
    {
        _isEnabled = status;

        onToggled?.Invoke(status);
    }
}
