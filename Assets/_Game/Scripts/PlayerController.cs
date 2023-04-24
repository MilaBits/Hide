using FishNet.Object;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float movementSpeed = 3;
    public float acceleration = 3;
    public float decelleration = .3f;

    private Rigidbody rb;
    private Controls controls;

    private float speed = 0;
    private Vector3 inputVector = Vector3.zero;

    public PlayerCamera playerCamera;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new Controls();
    }

    private void OnEnable() => controls.Gameplay.Enable();
    private void OnDisable() => controls.Gameplay.Disable();

    void Update()
    {
        if (!base.IsOwner) return;
        Vector2 input = controls.Gameplay.Movement.ReadValue<Vector2>();
        inputVector = new Vector3(input.x, 0, input.y).normalized;
    }

    private void FixedUpdate()
    {
        if (!base.IsOwner) return;

        if (inputVector != Vector3.zero)
        {
            speed = Mathf.Clamp(speed + (acceleration * Time.deltaTime), 0, movementSpeed);
            rb.velocity = inputVector * speed;
        }
        else
        {
            speed = Mathf.Clamp(speed - (decelleration * Time.deltaTime), 0, movementSpeed);
            rb.velocity = rb.velocity.normalized * speed;
        }
    }
}
