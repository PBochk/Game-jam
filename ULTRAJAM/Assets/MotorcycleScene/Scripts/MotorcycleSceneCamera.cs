using UnityEngine;

public class MotorcycleSceneCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 20, 0);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            Time.deltaTime * smoothSpeed
        );
    }
}
