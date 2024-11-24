using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    private Camera _mainCamera;
    private Plane _groundPlane;

    private void Awake()
    {
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
    }

    private void Look()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (_groundPlane.Raycast(ray, out float enter))
        {
            Vector3 targetPoint = ray.GetPoint(enter);
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.ToIso(), Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
            }
        }
    }

    private void Move()
    {
        Vector3 forwardMove = transform.forward * Input.GetAxisRaw("Vertical") * _speed * Time.deltaTime;
        Vector3 rightMove = transform.right * Input.GetAxisRaw("Horizontal") * _speed * Time.deltaTime;

        _rb.MovePosition(transform.position + forwardMove + rightMove);
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}