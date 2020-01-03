using UnityEngine;

public class EmptyCell : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    bool mutating;
    float mutateTimer;
    const float mutateChance = 0.025f;
    float mutateCooldown = 10f;
    const float mutateCooldownJiter = 1.5f;

    float targetAlpha;
    float mutationProgress;
    const float mutationStep = 0.01f;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        float rand = Random.Range(0f, 1f);

        Color color = spriteRenderer.color;

        if (rand < 0.2f)
        {
            color.a = Random.Range(0.05f, 0.15f);
            spriteRenderer.color = color;
        }
        else
        {
            color.a = 0f;
            spriteRenderer.color = color;
        }

        mutateCooldown = Random.Range(0, 2 * mutateCooldownJiter);
    }

    void Update()
    {
        Mutate();

        if (mutateTimer <= 0)
        {
            mutateTimer = mutateCooldown;
            mutateTimer += Random.Range(-mutateCooldownJiter, mutateCooldownJiter);

            if (DoMutation())
            {
                StartMutation();
            }
        }
        else
        {
            mutateTimer -= Time.deltaTime;
        }
    }

    bool DoMutation()
    {
        Color color = spriteRenderer.color;

        if (color.a <= 0.05f)
        {
            return Random.Range(0f, 1f) < mutateChance * 0.25f;
        }
        else
        {
            return Random.Range(0f, 1f) < mutateChance;
        }
    }

    void Mutate()
    {
        if (!mutating)
            return;

        Color color = spriteRenderer.color;
        color.a = Mathf.Lerp(color.a, targetAlpha, mutationProgress);
        spriteRenderer.color = color;

        mutationProgress += mutationStep;

        if (mutationProgress >= 1)
        {
            StopMutation();
        }
    }

    void StartMutation()
    {
        mutating = true;
        targetAlpha = Random.Range(0.0f, 0.2f);
        mutationProgress = 0;
    }

    void StopMutation()
    {
        mutating = false;
    }
}
