using System;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private Animator animator;
    
    private Vector2 move;
    private Vector2 look;
    private Vector3 rotationTarget;
    private float verticalVelocity;

    private CharacterController controller;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        //animator = GetComponentInChildren<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    
    private void Update()
    {
        movePlayer();
        rotatePlayer();
    }

    private void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        movement.y = VerticalVelocityCalculation();
        controller.Move(movement.ToIso() * moveSpeed * Time.deltaTime);
        float forwardValue = Vector3.Dot(movement, transform.right.ToIso());
        animator.SetFloat("Walk", forwardValue);
    }
    private float VerticalVelocityCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }

    private void rotatePlayer()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(look);
        if (Physics.Raycast(ray, out hit))
        {
            rotationTarget = hit.point;
        }
        else
        {
            rotationTarget = transform.position + transform.forward * 10f;
        }

        Vector3 lookPos = rotationTarget - transform.position;
        lookPos.y = 0;

        if (lookPos != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            Debug.DrawRay(transform.position, transform.forward * 3f, Color.red);
        }
    }

    
}
public static class Helpers 
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
