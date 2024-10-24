using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCharacter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;
    
    [Header("Animator Parameters")]
    [SerializeField] [Min(0.5f)] private float waitBeforeCheckRdnAnim;
    [SerializeField] [Range(0f, 1f)] private float chancePlayAnimationRnd;
    
    private void OnEnable()
    {
        StartCoroutine(RandomCheckAnimation());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        animator.SetTrigger("IsReset");
    }
        
    public void PlayThanksAnimation()
    {
        animator.SetTrigger("IsPicked");
    }

    private IEnumerator RandomCheckAnimation()
    {
        yield return new WaitForSeconds(Random.Range(0.1f,2f));
        while (enabled)
        {
            yield return new WaitForSeconds(waitBeforeCheckRdnAnim);
            if (Random.Range(0f, 1f) > chancePlayAnimationRnd) animator.SetTrigger("IsHappy");
        }
    }
}
