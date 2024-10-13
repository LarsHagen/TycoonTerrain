using UnityEngine;

/// <summary>
/// Basic free cam that mimics the free camera in the scene view
/// </summary>
public class FreeCam : MonoBehaviour
{
    public float movementSpeed = 15;
    public float rotationSpeed = 30;
    public float speedModifier = 2;

    private float _rotationX;
    private float _rotationY;

    void Update()
    {
        if (!Input.GetMouseButton(1))
            return;

        _rotationY += Input.mousePositionDelta.x * Time.deltaTime * rotationSpeed;
        _rotationX -= Input.mousePositionDelta.y * Time.deltaTime * rotationSpeed;

        transform.eulerAngles = new Vector3(_rotationX, _rotationY, 0);

        Vector3 moveDirection = new();

        if (Input.GetKey(KeyCode.W))
            moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.D))
            moveDirection += transform.right;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.E))
            moveDirection += transform.up;
        if (Input.GetKey(KeyCode.Q))
            moveDirection -= transform.up;

        moveDirection.Normalize();
        moveDirection *= Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
            moveDirection *= speedModifier;

        transform.position += moveDirection * movementSpeed;
    }
}
