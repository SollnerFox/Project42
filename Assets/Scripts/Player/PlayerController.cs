using System;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private Animator animator;
    
    private Vector2 move;
    private Vector2 look;
    private Vector3 rotationTarget;
    private float verticalVelocity;

    private CharacterController controller;
    private void Awake() => controller = GetComponent<CharacterController>();
    

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
        if (movement.magnitude > 0f)
        {
            movement.Normalize();
            controller.Move(movement.ToIso() * moveSpeed * Time.deltaTime);
        }
        float velocityZ = Vector3.Dot(movement, transform.forward);
        float velocityX = Vector3.Dot(movement, transform.right);
        animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
    }
    private float VerticalVelocityCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
            animator.CrossFade("Movement", 0.1f);
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            animator.CrossFade("Fall", 0.1f);
        }
        return verticalVelocity;
    }

    private void rotatePlayer()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(look);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            var direction = hit.point - transform.position;
            direction.y = 0f;
            //direction.Normalize();
            transform.forward = direction;
            Debug.DrawRay(transform.position, transform.forward * 3f, Color.red);
        }  
    }
    

    
}
public static class Helpers 
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
