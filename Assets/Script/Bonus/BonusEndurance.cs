using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEndurance : MonoBehaviour
{
    [SerializeField] private float amout=10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerController>().Endurance += amout; 
        Destroy(this.gameObject);
    }
}
