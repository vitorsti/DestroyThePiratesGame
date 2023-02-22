using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeteriorationAnimationController : MonoBehaviour
{
    Animator animator;
    public HealthManager healthManager;
    //float healthController = 100;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        healthManager = GetComponentInParent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("health", healthManager.health);
    }

    public void Reset()
    {
        animator.Play("idle");
    }
}
