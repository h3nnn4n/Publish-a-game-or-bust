using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject sink;
    GameController gameController;

    public float health = 10;
    public float speed = 1f;
    public float credits = 10f;

    readonly float speedScale = 0.1f;

    void Start()
    {
        FindAndCacheSink();
        gameController = FindGameController();
    }

    GameController FindGameController()
    {
        GameObject gridObject = GameObject.FindGameObjectWithTag("Grid");

        return gridObject.GetComponent<GameController>();
    }

    void Update()
    {
        FindAndCacheSink();
        Despawn();
        MoveTowardsSink();
    }

    void Despawn()
    {
        Vector3 sinkPosition = sink.transform.position;
        Vector3 selfPosition = transform.position;
        Vector3 distance = sinkPosition - selfPosition;

        if (distance.magnitude < 1.0)
        {
            Destroy(gameObject);
        }

        if (health <= 0)
        {
            gameController.AddCredits(credits);
            Destroy(gameObject);
        }
    }

    void MoveTowardsSink()
    {
        Vector3 sinkPosition = sink.transform.position;
        Vector3 selfPosition = transform.position;
        Vector3 moveDirection = sinkPosition - selfPosition;
        Vector3 step = moveDirection.normalized * speed * speedScale;

        transform.position += step;
    }

    void FindAndCacheSink()
    {
        if (sink == null)
        {
            sink = GameObject.FindGameObjectWithTag("Sink");
        }
    }

    public void DealDamage(float damage)
    {
        health -= damage;
    }
}
