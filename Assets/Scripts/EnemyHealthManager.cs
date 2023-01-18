using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public int maxHp = 1;
    public int CurrentHp;

    public int deathSound;

    public GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        CurrentHp--;

        if(CurrentHp <= 0)
        {
            AudioManager.instance.PlaySFX(deathSound);

            Destroy(gameObject);

            PlayerController.instance.Bounce();

            Instantiate(deathEffect, transform.position, transform.rotation);
        }
    }
}
