using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemiesInBeach : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemy = new();


    private void Update()
    {
        if (InfoManager.Instance.currentMissionIndex != 0) return;
        foreach (GameObject enemy in _enemy)
        {
            if (enemy.activeSelf) return;
        }
        InfoManager.Instance.SetMissionIndex(1);
    }
}
