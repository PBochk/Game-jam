using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TopDownBikeController3D : MonoBehaviour
{
    [Header("References")]
    public Transform model; // сюда перетащить дочерний объект Model

    [Header("Movement")]
    public float maxSpeed = 8f;
    public float acceleration = 10f;

    [Header("Rotation")]
    public float rotationDelay = 6f;
    public float maxLeanAngle = 20f;
    public float leanSpeed = 6f;

    private float currentSpeed = 0f;
    private float currentLean = 0f;

    private Rigidbody rb;
    private Camera mainCam;
    private float currentYRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        HandleAcceleration();
        RotateToMouse();
    }

    void FixedUpdate()
    {
        Move();
    }

    void RotateToMouse()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (!groundPlane.Raycast(ray, out float distance)) 
            return;

        Vector3 point = ray.GetPoint(distance);
        Vector3 dir = point - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        // целевой поворот ТОЛЬКО по Y
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * rotationDelay
        );

        // угол для наклона
        float angleDiff = Vector3.SignedAngle(transform.forward, dir, Vector3.up);

        float targetLean = Mathf.Clamp(angleDiff, -maxLeanAngle, maxLeanAngle);
        currentLean = Mathf.Lerp(currentLean, -targetLean, Time.deltaTime * leanSpeed);

        // Наклоняем ТОЛЬКО модель (локальная ось X — как "мотоцикл")
        if (model != null)
        {
            Vector3 e = model.localEulerAngles;
            e.z = currentLean; // currentLean — в градусах
            model.localEulerAngles = e;
        }
    }

    void HandleAcceleration()
    {
        if (Input.GetKey(KeyCode.W))
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        else
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
    }

    void Move()
    {
        rb.linearVelocity = transform.forward * currentSpeed;
    }
}