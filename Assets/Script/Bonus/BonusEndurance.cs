using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEndurance : MonoBehaviour
{
    [SerializeField] private float amout=10;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerController>().Endurance += amout;
        animator.SetBool("ispick", true);
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
