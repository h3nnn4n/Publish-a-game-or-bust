using UnityEngine;
using System;

public class HealthBar
{
    GameObject enemyGameObject;
    GameObject healthBarGameObject;
    SpriteRenderer healthBarSpriteRenderer;
    Enemy enemy;
    float maxHealth;
    float currentHealth;
    float baseScale;

    public HealthBar(GameObject parent)
    {
        enemyGameObject = parent;
        enemy = parent.GetComponent<Enemy>();
        healthBarSpriteRenderer = GetSpriteRenderer();
        maxHealth = enemy.health;
        currentHealth = enemy.GetCurrentHealth();
        healthBarGameObject = GetHealthBarGameObject();
        baseScale = healthBarGameObject.transform.localScale.x;
    }

    public void Update()
    {
        if (Math.Abs(enemy.GetCurrentHealth() - currentHealth) < 1e-6)
            return;

        currentHealth = enemy.GetCurrentHealth();
        float healthPercent = currentHealth / maxHealth;

        healthBarSpriteRenderer.color = Color.Lerp(
            Color.red,
            Color.green,
            healthPercent
        );

        Vector3 position = healthBarSpriteRenderer.transform.localPosition;
        position.x = Mathf.Lerp(
            0,
            -0.4f,
            1 - healthPercent
        );

        Vector3 scale = healthBarSpriteRenderer.transform.localScale;
        scale.x = Mathf.Lerp(
            baseScale,
            0,
            1 - healthPercent
        );

        healthBarSpriteRenderer.transform.localPosition = position;
        healthBarSpriteRenderer.transform.localScale = scale;
    }

    GameObject GetHealthBarGameObject()
    {
        return enemyGameObject.transform.Find("HealthBar").gameObject;
    }

    SpriteRenderer GetSpriteRenderer()
    {
        return GetHealthBarGameObject().GetComponent<SpriteRenderer>();
    }
}
