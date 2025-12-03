using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{

    [Range(0, 100)]
    [SerializeField]
    float _speed;

    [Range(1, 600)]
    [SerializeField]
    float _jumpForce;

    [Range(0.1f, 1f)]
    [SerializeField]
    float _maxJumpTime, _minJumpPercentage;
    float _jumpPercentage;

    Vector2 _movement;

    Rigidbody2D _rb;

    bool _grounded, _jumpPressed, _facingRight;

    [SerializeField]
    Transform _groundCheck;

    Animator _animator;

    float _lastYVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._rb = GetComponent<Rigidbody2D>();
        _jumpPercentage = _minJumpPercentage;
        _facingRight = false;
        _animator = GetComponent<Animator>();
        _lastYVelocity = 0;
    }

    public void Update()
    {
        if (_facingRight && _movement.x > 0)
            Flip();
        else if (!_facingRight && _movement.x < 0)
            Flip();
        //if we're in air we can descend
        _rb.position = _rb.position + new Vector2(_movement.x * Time.deltaTime * _speed, _grounded || _jumpPressed ? 0 : Mathf.Min(0, _movement.y) * Time.deltaTime * _speed * 1.4f);
        _animator.SetFloat("MovementX", _movement.x);
        //_animator.SetFloat("MovementY", _movement.y);
        _animator.SetFloat("MovementY", _rb.linearVelocityY);
        
        if (_jumpPressed)
        {
            _jumpPercentage += Time.deltaTime;
            //Mathf.Clamp(_jumpPercentage, 0, _maxJumpTime);
            float percentage = Mathf.Clamp(_jumpPercentage / _maxJumpTime, 0, 1);
            if (percentage < 1) {
                _rb.AddForce(new Vector2(0, Mathf.Lerp(_jumpForce, 0, percentage)));    
            }
        }
        //_rb.AddForce(new Vector2(_movement.x * Time.deltaTime * _speed, 0));
        // Debug.Log(_movement);
    }

    public void Move(CallbackContext context)
    {
        _movement = context.action.actionMap["Move"].ReadValue<Vector2>();
    }

    public void Jump(CallbackContext context)
    {
        if (context.action.IsPressed())
        {
            if (_grounded)
                _jumpPressed = true;
                _animator.SetBool("Jumping", true);
        } else {
            _jumpPressed = false;
            _jumpPercentage = _minJumpPercentage;
            _animator.SetBool("Jumping", false);
        }
  
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            _grounded = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            _grounded = false;
    }

    public void Flip() {
        this.transform.eulerAngles = new Vector2(0, _facingRight? 180 : 0);
        _facingRight = !_facingRight;
    }
}
