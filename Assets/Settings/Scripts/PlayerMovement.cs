using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{

    [Range(0, 100)]
    [SerializeField]
    float _speed;

    [Range(0, 50)]
    [SerializeField]
    float _jumpForce;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Move(CallbackContext context)
    {
        Vector2 movement = context.action.actionMap["Move"].ReadValue<Vector2>();

        Debug.Log(movement);
    }
}
