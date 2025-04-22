using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFader : MonoBehaviour
{
    public Animator animator;

    public void FadeOut()
    {
        animator.SetTrigger("startfadeout");
    }
}

