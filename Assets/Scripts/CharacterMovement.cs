using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private Vector3 _offset;

    private Rigidbody2D _rb;
    private bool _isGrounded;
    private float rotate;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();
        Direction();
    }

    void Movement()
    {
        // horizontal movement
        float horizontalMove = (Input.GetAxisRaw("Horizontal") * _speed);

        // vertical movement
        bool isJumping = Input.GetAxisRaw("Vertical") > 0;
        float jumpMove = _rb.velocity.y;
        if (isJumping && _isGrounded)
            jumpMove = _jumpForce;

        // initiate the actual movement
        _rb.velocity = new Vector2(horizontalMove, jumpMove);
    }

    void Direction()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
            rotate = 90;
        else if (Input.GetAxisRaw("Horizontal") > 0)
            rotate = 0;
        transform.rotation = new Quaternion(0, rotate, 0, transform.rotation.w);
    }

    public void GroundCheck(bool grounded)
    {
        _isGrounded = grounded;
    }
}