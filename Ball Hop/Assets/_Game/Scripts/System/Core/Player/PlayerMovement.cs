using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _jumpHeight = 5f;
    [SerializeField] float _jumpTime = 1f;
    [SerializeField] float _jumpDistance = 8f;
    [SerializeField] float _maxPosX = 3f;

    [Space]
    [Range(0, 1)] [SerializeField] float _smoothHorizontalTime = 0.2f;

    float _xVelocity;
    float _yVelocity;
    float _yGravity;

    float _elapsedTime = 0;
    float _startVal, _endVal;

    Vector3 _playerPos;

    Rigidbody _rb;

    float GetJumpHalfTime => _jumpTime * .5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerInput.OnPointerDrag += HorizontalMovement;
    }
    private void OnDisable()
    {
        PlayerInput.OnPointerDrag -= HorizontalMovement;
    }

    private void Start()
    {
        // calculate vertical force
        _yVelocity = (_jumpHeight / GetJumpHalfTime) * 2;

        // calculate & apply gravity
        _yGravity = -_yVelocity / GetJumpHalfTime;
        Physics.gravity = Vector3.up * _yGravity;
    }

    private void Update()
    {
        // Update horizontal movement input
        float input = Input.GetAxisRaw("Horizontal"); // -1 (kiri), 0, 1 (kanan)
        HorizontalMovement(input);

        // Jump progress
        _elapsedTime += Time.deltaTime;
        float timePercentage = _elapsedTime / _jumpTime;
        Move(timePercentage);
    }

    public void Jump()
    {
        _elapsedTime = 0;
        _startVal = transform.position.z;
        _endVal += _jumpDistance;

        // apply jump
        _rb.velocity = new Vector3(_rb.velocity.x, _yVelocity, _rb.velocity.z);
    }

    internal void Revive(Vector3 position)
    {
        _endVal -= _jumpDistance;
        _playerPos.x = position.x;
        transform.position = new Vector3(_playerPos.x, 0, _endVal);
    }

    public void HorizontalMovement(float direction)
    {
        _playerPos.x += direction * _maxPosX * Time.deltaTime; // gerak terus selama tombol ditekan
        _playerPos.x = Mathf.Clamp(_playerPos.x, -_maxPosX, _maxPosX); // batas kiri-kanan
    }

    
    private void Move(float percentage)
    {
        if (transform.position.z == _endVal) return;

        _playerPos.z = Mathf.Lerp(_startVal, _endVal, percentage);
        float xPos = Mathf.SmoothDamp(transform.position.x, _playerPos.x, ref _xVelocity, _smoothHorizontalTime);

        transform.position = new Vector3(xPos, transform.position.y, _playerPos.z);
    }
}
