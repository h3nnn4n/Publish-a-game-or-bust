using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject target;
    GameController gameController;

    public float timeToLive = 0.7f;

    float lineWidth = 0.05f;
    float bulletTimer;

    void Start()
    {
        gameController = GameController.GetInstance();
        bulletTimer = timeToLive;
    }

    void Update()
    {
        for (int i = 0; i < gameController.GetGameSpeed(); i++)
        {
            BulletUpdate();
        }
    }

    void BulletUpdate()
    {
        bulletTimer -= Time.deltaTime;

        FadeOut();

        if (bulletTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target_)
    {
        target = target_;

        Draw();
    }

    void Draw()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        Vector3[] points = new Vector3[2];
        points[0] = Vector3.zero;
        points[1] = target.transform.position - transform.position;

        lineRenderer.SetPositions(points);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    void FadeOut()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;


        startColor.a *= 0.85f;
        endColor.a *= 0.85f;

        lineRenderer.endColor = startColor;
        lineRenderer.endColor = endColor;
    }
}
