using System;
using System.Collections;
using System.Collections.Generic;
using Core.Enums;
using Core.Tools;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerEntity : MonoBehaviour
    {
        [Header("HorizontalMovement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private Direction _direction;

        [Header("VerticalMovement")] 
        [SerializeField] private float _verticakSpeed;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private float _maxVerticalPosition;
        [SerializeField] private float _minVerticalPosition;

        [Header("Jump")] 
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _gravityScale;

        [SerializeField] private DirectionalCameraPair _cameras;
        

        private Rigidbody2D _rigidbody;

        private float _sizeModificator;
        private bool _isJumping;
        private float _startJumpVerticalPosition;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            float positionDifference = _maxVerticalPosition - _minVerticalPosition;
            float sizeDifference = _maxSize - _minSize;
            _sizeModificator = sizeDifference / positionDifference;
            UpDateSize();
        }

        private void Update()
        {
            if(_isJumping)
              UpdateJump(); 
        }

        public void MoveHorizontally(float direction)
        {
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _horizontalSpeed;
            _rigidbody.velocity = velocity;
        }

        public void MoveVertically(float direction)
        {
            if(_isJumping)
                return;
            
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = direction * _verticakSpeed;
            _rigidbody.velocity = velocity;
            
            if (direction == 0)
                return;

            float verticalPosition = Mathf.Clamp(transform.position.y, _minVerticalPosition, _maxVerticalPosition);
            _rigidbody.position = new Vector2(_rigidbody.position.x, verticalPosition);
            UpDateSize();
        }

        public void Jump()
        {
            if (_isJumping)
                return;

            _isJumping = true;
            _rigidbody.AddForce(Vector2.up*_jumpForce);
            _rigidbody.gravityScale = _gravityScale;
            _startJumpVerticalPosition = transform.position.y;
        }

        private void UpDateSize()
        {
            float verticalDelta = _maxVerticalPosition - transform.position.y;
            float currentSizeModificator = _minSize + _sizeModificator * verticalDelta;
            transform.localScale = Vector2.one * currentSizeModificator;
        }
        
        private void SetDirection(float direction)
        {
            if((_direction == Direction.Right && direction < 0) || 
               (_direction ==Direction.Left && direction >0))
                Flip();
        }

        private void Flip()
        {
            transform.Rotate(0,180,0);
            _direction = _direction == Direction.Right ? Direction.Left : Direction.Right;
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key == _direction;
        }

        private void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && _rigidbody.position.y <= _startJumpVerticalPosition)
            {
                ResetJump();
                return;
            }
        }

        private void ResetJump()
        {
            _isJumping = false;
            _rigidbody.position = new Vector2(_rigidbody.position.x, _startJumpVerticalPosition);
            _rigidbody.gravityScale = 0;
        }
    }
}
