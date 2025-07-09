using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _onCheckPointSprite;
    [SerializeField] private Sprite _offCheckPointSprite;
    public int checkPointIndex;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = _offCheckPointSprite;

        if(InfoManager.Instance.currentCheckpointIndex >= checkPointIndex)
        {
            _spriteRenderer.sprite = _onCheckPointSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (InfoManager.Instance.currentCheckpointIndex != checkPointIndex - 1) return;
            SoundsManager.GetInstance().PlayClip(SoundsManager.Sound.Checkpoint);
            _spriteRenderer.sprite = _onCheckPointSprite;
            InfoManager.Instance.currentCheckpointIndex = checkPointIndex;
            InfoManager.Instance.NewPlayerPosition = this.transform.position;

            if(checkPointIndex == 1 && SceneManager.GetActiveScene().name == "Forest")
            {
                if (InfoManager.Instance.currentMissionIndex >= 1) return;
                InfoManager.Instance.SetMissionIndex(1);
            }

            if (checkPointIndex == 2 && SceneManager.GetActiveScene().name == "Forest")
            {
                if (InfoManager.Instance.currentMissionIndex >= 3) return;
                InfoManager.Instance.SetMissionIndex(3);
            }
            Player.instance.SavePlayer();
        }
    }


}
