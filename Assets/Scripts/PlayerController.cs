using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //[SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    
    private CharacterController _controller;
    private Camera _mainCamera;
    private Plane _groundPlane;

    private float _currentAttractionCharacter = 0f;
    private float _gravityForce = 20f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        Look();
    }

    private void FixedUpdate()
    {
        Move();
        GravityHadling();
    }

    private void Look()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        /*if (_groundPlane.Raycast(ray, out float enter))
        {
            Vector3 targetPoint = ray.GetPoint(enter);
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.ToIso(), Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
            }
        }*/
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDirection = new Vector3(hit.point.x - transform.position.x, 0, hit.point.z - transform.position.z);
            transform.LookAt(lookDirection);
        }
    }

    private void Move()
    {
        Vector3 moveDirection =
            (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")) *
            _speed * Time.deltaTime + Vector3.up * _currentAttractionCharacter * Time.deltaTime;
        _controller.Move(moveDirection);
    }

    private void GravityHadling()
    {
        if (!_controller.isGrounded) _currentAttractionCharacter -= _gravityForce * Time.deltaTime;
        else _currentAttractionCharacter = 0f;
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}