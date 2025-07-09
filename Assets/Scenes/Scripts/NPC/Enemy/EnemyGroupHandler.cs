using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupHandler : MonoBehaviour
{
    private List<EnemyMovement> _enemyMovements = new();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            _enemyMovements.Add(child.gameObject.GetComponent<EnemyMovement>());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (EnemyMovement enemyMovement in _enemyMovements)
            {
                enemyMovement.OnSetPlayerInRangeValue(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (EnemyMovement enemyMovement in _enemyMovements)
            {
                enemyMovement.OnSetPlayerInRangeValue(false);
            }

        }
    }
}
