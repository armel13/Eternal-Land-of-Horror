using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState
{
    Idle,
    Chase,
    Attack,
    Stagger,
    Patrol
}

public class Boss : MonoBehaviour, IDamageable
{
    [SerializeField] private Image _healthFill;
    public event Action<float, float> OnTakeDamage;

    public BossState currentState;

    public float maxHealth = 2f;
    public float currHealth;
    //public string enemyName;
    public int baseAttack;
    public GameObject deathFX;
    [HideInInspector]
    public FireProjectiles shooter;
    Collectables newItem;


    protected virtual void Awake()
    {
        ChangeState(BossState.Idle);
        currHealth = maxHealth;
        _healthFill.fillAmount = (currHealth / maxHealth);
        shooter = GetComponent<FireProjectiles>();
        newItem = GetComponent<Collectables>();
    }

    protected virtual void OnEnable()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage(float damageTaken, GameObject damageGiver)
    {
        currHealth -= damageTaken;
        _healthFill.fillAmount = (currHealth / maxHealth);
        if (currHealth <= 0f)
        {
            Destroy();
            if(AfterKuntiMerahDefeatedDialogue.GetInstance() != null) AfterKuntiMerahDefeatedDialogue.GetInstance().StartAfterKuntiMerahDefeatedDialogue();
            // Memanggil metode pada UIManager ketika health boss habis
            //UIManager.instance.BossDefeated();
        }
        else
        {
            Knockback kB = GetComponent<Knockback>();
            if (kB!= null)
            {
                ChangeState(BossState.Stagger);
                StartCoroutine(kB.KnockBack(damageGiver.transform));
                OnTakeDamage?.Invoke(currHealth, damageTaken);
            }
        }
    }

    public virtual void Destroy()
    {

        gameObject.SetActive(false);
        GameObject deathEffect = Instantiate(deathFX, transform.position, Quaternion.identity);
        GameObject droppedItem = newItem.GetRandomItem();
        if (droppedItem != null)
            Instantiate(droppedItem, transform.position, Quaternion.identity);
        Destroy(deathEffect, 1f);
    }

    public void ChangeState(BossState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }

}
