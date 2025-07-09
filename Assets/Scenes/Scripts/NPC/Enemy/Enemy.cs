using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Stagger,
    Patrol
}

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Image _healthFill;
    public EnemyState currentState;
    public int EnemyID;
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
        ChangeState(EnemyState.Idle);
        currHealth = maxHealth;
        shooter = GetComponent<FireProjectiles>();
        newItem = GetComponent<Collectables>();
    }

    private void Start()
    {
        _healthFill.fillAmount = (currHealth/maxHealth);
        StartCoroutine(DelayCheckObjectStatus());
    }

    IEnumerator DelayCheckObjectStatus()
    {
        yield return new WaitForSeconds(0.2f);

        if (!InfoManager.Instance.EnemiesID.Contains(EnemyID))
        {
            //Debug.Log("No enemy",this);
            gameObject.SetActive(false);
        }

        if(gameObject.activeInHierarchy) StartCoroutine(DelayCheckObjectStatus());
    }

    protected virtual void OnEnable()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage(float damageTaken, GameObject damageGiver)
    {
        currHealth -= damageTaken;
        if (currHealth > 0) _healthFill.fillAmount = (currHealth / maxHealth);
        else _healthFill.fillAmount = 0;
        if (currHealth <= 0f)
        {
            if (InfoManager.Instance.EnemiesID.Contains(EnemyID)) InfoManager.Instance.RemoveEnemiesID(EnemyID);
            Destroy();
        }
        else
        {
            Knockback kB = GetComponent<Knockback>();
            if (kB!= null)
            {
                ChangeState(EnemyState.Stagger);
                StartCoroutine(kB.KnockBack(damageGiver.transform));
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

        // Menghapus objek dari daftar spawnedEnemies jika terkait dengan WaveSpawner
        if (GameObject.FindGameObjectWithTag("WaveSpawner") != null)
        {
            WaveSpawner waveSpawner = GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>();
            waveSpawner.RemoveEnemy(gameObject);

            // Hancurkan objek setelah sedikit penundaan
            Destroy(gameObject, 0.1f);
        }
    }



    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }

}
