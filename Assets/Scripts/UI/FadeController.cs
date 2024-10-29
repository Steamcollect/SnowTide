using UnityEngine;

public class FadeController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private RSE_FadeInOut rseFadeInOut;

    private void OnEnable() => rseFadeInOut.action += FadeInOut;
    private void OnDisable() => rseFadeInOut.action -= FadeInOut;

    private void FadeInOut(bool fadeIn)
    {
        animator.SetTrigger(fadeIn? "FadeIn" : "FadeOut");
    }
}
