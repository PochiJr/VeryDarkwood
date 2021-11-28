using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitAnimations : MonoBehaviour
{
    public Animator animator;

    void exitSlashAnimation() { animator.SetBool("isAttacking", false); }
    void exitTakingDamageAnimation() { animator.SetBool("isTakingDamage", false); }

}


