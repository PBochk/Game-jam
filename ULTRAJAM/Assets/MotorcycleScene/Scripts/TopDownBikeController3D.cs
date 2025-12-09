using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BikeController : MonoBehaviour
{
    [Header("References")]
    public Transform model; // дочерняя модель байка

    [Header("Movement")]
    public float maxSpeed = 10f;
    public float acceleration = 12f;

    [Header("Rotation")]
    public float rotationSpeed = 8f;
    public float maxLeanAngle = 20f;
    public float leanSpeed = 6f;

    private Rigidbody rb;
    private Camera mainCam;

    private float currentSpeed;
    private float currentLean;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        RotateToMouse();
        HandleAcceleration();
    }

    void FixedUpdate()
    {
        Move();
    }

    void RotateToMouse()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (!plane.Raycast(ray, out float dist))
            return;

        Vector3 hitPoint = ray.GetPoint(dist);
        Vector3 dir = hitPoint - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        // Поворот только по Y
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * rotationSpeed
        );

        // Расчёт наклона (визуального)
        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        float targetLean = Mathf.Clamp(-angle, -maxLeanAngle, maxLeanAngle);
        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);

        // Наклоняем ТОЛЬКО модель
        if (model != null)
        {
            model.localRotation = Quaternion.Euler(0f, 0f, currentLean);
        }
    }

    void HandleAcceleration()
    {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
    }

    void Move()
    {
        rb.linearVelocity = transform.forward * currentSpeed;
    }
}
