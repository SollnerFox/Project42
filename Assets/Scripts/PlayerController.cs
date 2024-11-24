using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    

    private float _currentAttractionCharacter = 0f;
    private float _gravityForce = 20f;

    private CharacterController _controller;
    private Camera _mainCamera;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Look();
    }

    private void FixedUpdate()
    {
        Move();
        GravityHandling();
    }

    private void Look()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDirection = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookDirection);
        }
    }

    private void Move()
    {
        Vector3 moveDirection =
            ((transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")) *
            _speed  + Vector3.up * _currentAttractionCharacter ) * Time.deltaTime;
        _controller.Move(moveDirection);
    }

    private void GravityHandling()
    {
        if (!_controller.isGrounded) _currentAttractionCharacter -= _gravityForce * Time.deltaTime;
        else _currentAttractionCharacter = 0f;
    }
}