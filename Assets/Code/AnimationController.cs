using UnityEngine;

public class AnimationController : MonoBehaviour
{
    static AnimationController instance;
    public Animator animator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one AnimationController exists!");
            DestroyImmediate(this);
        }
    }

    public void TriggerFadeIn()
    {
        animator.SetTrigger("FadeIn");
        Debug.Log("FadeIn triggered");
    }

    public void TriggerFadeOut()
    {
        animator.SetTrigger("FadeOut");
        Debug.Log("FadeOut triggered");
    }

    public void FinishedFadeIn()
    {
        Debug.Log("FadeIn finished");
    }

    public void FinishedFadeOut()
    {
        Debug.Log("FadeOut finished");
    }

    static public AnimationController GetInstance()
    {
        return instance;
    }
}
