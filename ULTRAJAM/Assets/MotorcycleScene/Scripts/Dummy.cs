using UnityEngine;
using System;

public class Dummy : MonoBehaviour
{
    private Transform player;
    private bool isAlive = false;

    public Action<Dummy> OnDeath;

    public float turnSpeed = 4f;
    public float maxDistanceFromPlayer = 120f; // дистанция самоуничтожения

    public void Init(Transform playerRef)
    {
        player = playerRef;
    }

    public void Spawn(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        isAlive = true;
    }

    void Update()
    {
        if (!isAlive || !player) return;

        // ----- автодеактивация по дистанции -----
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > maxDistanceFromPlayer)
        {
            DestroySelf();
            return;
        }

        // ----- поворот к игроку -----
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion target = Quaternion.LookRotation(dir);
        Vector3 e = target.eulerAngles;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(0f, e.y, 0f),
            Time.deltaTime * turnSpeed
        );
    }

    public void Crush()
    {
        if (!isAlive) return;

        isAlive = false;
        gameObject.SetActive(false);
        OnDeath?.Invoke(this);
    }

    void DestroySelf()
    {
        isAlive = false;
        gameObject.SetActive(false);
        OnDeath?.Invoke(this);
    }
}