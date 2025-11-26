using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{

    [Range(0, 100)]
    [SerializeField]
    float _speed;

    [Range(50, 600)]
    [SerializeField]
    float _jumpForce;

    [Range(0.1f, 1f)]
    [SerializeField]
    float _maxJumpTime, _minJumpPercentage;
    float _jumpPercentage;

    Vector2 _movement;

    Rigidbody2D _rb;

    bool _grounded, _jumpPressed;

    [SerializeField]
    Transform _groundCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._rb = GetComponent<Rigidbody2D>();
        _jumpPercentage = _minJumpPercentage;
    }

    public void Update()
    {
        //if we're in air we can descend
        _rb.position = _rb.position + new Vector2(_movement.x * Time.deltaTime * _speed, _grounded ? 0 : Mathf.Min(0, _movement.y) * Time.deltaTime * _speed);
        if (_jumpPressed)
        {
            _jumpPercentage += Time.deltaTime;
            Mathf.Clamp(_jumpPercentage, 0, _maxJumpTime);
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
        if (context.control.IsPressed())
        {
            _jumpPressed = true;
        }
        else
        {
            if (_grounded)
            {


            
                    Debug.Log(_jumpPercentage);
                    float percentage = Mathf.Clamp(_jumpPercentage / _maxJumpTime, 0, 1);
                    _rb.AddForce(new Vector2(0, _jumpForce * percentage));
                    _jumpPercentage = _minJumpPercentage;
                    _jumpPressed = false;
            }
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
}
