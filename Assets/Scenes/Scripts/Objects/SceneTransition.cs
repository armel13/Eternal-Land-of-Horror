using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTransition : MonoBehaviour
{
    // Nama adegan yang akan dimuat setelah transisi
    public string sceneToLoad;
    // Posisi awal pemain setelah masuk ke adegan baru
    public Vector2 playerStartPosition;

    // GameObject yang mewakili efek fade out saat transisi
    public GameObject fadeOutImage;
    // Waktu tunda sebelum transisi selesai
    public float fadeWait = .33f;

    [SerializeField] private CanvasGroup _openingPanelCanvasGroup;
    private void Start()
    {
        if (_openingPanelCanvasGroup == null) return;
        _openingPanelCanvasGroup.alpha = 0;
        _openingPanelCanvasGroup.gameObject.SetActive(false);
        if (!SaveManager.GetInstance().IsLevelStartTransitionOn) return;
        _openingPanelCanvasGroup.gameObject.SetActive(true);
        SaveManager.GetInstance().ChangeIsLevelStartTransitionOnValue(false);
        _openingPanelCanvasGroup.alpha = 1.0f;
        _openingPanelCanvasGroup.DOFade(0, 0.5f).SetDelay(1f).OnComplete(() =>
        {
            _openingPanelCanvasGroup.gameObject.SetActive(false);
        }) ;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Memeriksa apakah objek yang masuk ke area trigger adalah pemain
        if (other.CompareTag("Player"))
        {
            if (string.Equals("HouseInterior", sceneToLoad))
            {
                InfoManager.Instance.isFromHouseInterior = true;
            }

            if (string.Equals("House1", sceneToLoad))
            {
                InfoManager.Instance.isFromHouseHouse1 = true;
            }

            if (string.Equals("House2", sceneToLoad))
            {
                InfoManager.Instance.isFromHouseHouse2 = true;
            }

            if (!InfoManager.Instance.canEnterHouse3)
            {
                if ((string.Equals("House3", sceneToLoad) && Inventory.GetInstance().commonKeys <= 0))
                {
                    Debug.Log("Silver keys tidak ada di dalam inventory");
                    return;
                }
                else if (string.Equals("House3", sceneToLoad))
                {
                    InfoManager.Instance.isFromHouseHouse3 = true;
                    InfoManager.Instance.canEnterHouse3 = true;
                    Inventory.GetInstance().RemoveItem(ItemType.CommonKey);
                }
            }
            else if (InfoManager.Instance.canEnterHouse3 && (string.Equals("House3", sceneToLoad)))
            {
                InfoManager.Instance.isFromHouseHouse3 = true;
            }

            if (!InfoManager.Instance.canEnterHouse4)
            {
                if ((string.Equals("House4", sceneToLoad) && Inventory.GetInstance().uncommonKeys <= 0))
                {
                    Debug.Log("Red keys tidak ada di dalam inventory");
                    return;
                }
                else if (string.Equals("House4", sceneToLoad))
                {
                    InfoManager.Instance.isFromHouseHouse4 = true;
                    InfoManager.Instance.canEnterHouse4 = true;
                    Inventory.GetInstance().RemoveItem(ItemType.UncommonKey);
                }
            }
            else if (InfoManager.Instance.canEnterHouse4 && (string.Equals("House4", sceneToLoad)))
            {
                InfoManager.Instance.isFromHouseHouse4 = true;
            }

            // Menyimpan posisi awal pemain ke InfoManager
            if (string.Equals("Island", sceneToLoad))
            {
                if (!string.Equals(SceneManager.GetActiveScene().name, "Forest"))
                {
                    Debug.Log("Save Position after exiting house");
                    InfoManager.Instance.NewPlayerPosition = playerStartPosition;
                }
                else
                {
                    if (InfoManager.Instance.currentMissionIndex < 4) return; //Cant advance to level 2 without clearing the last mission
                    InfoManager.Instance.NewPlayerPosition = new Vector2(-8.63f, -9.95f);
                    InfoManager.Instance.currentCheckpointIndex = 0;
                    InfoManager.Instance.currentMissionIndex = 0;
                    SaveManager.GetInstance().UpdateSavedDuration(Mathf.RoundToInt(901));
                    Player.instance.SavePlayer(); //Save when transition to level 2
                }
            }
            else
            {
                //InfoManager.Instance.NewPlayerPosition = playerStartPosition;
            }

            if (string.Equals("BossCave", sceneToLoad))
            {
                Player.instance.SavePlayer();
            }


            // Memperbarui statistik pemain
            InfoManager.Instance.UpdateStats();

            // Memulai transisi dengan fade
            StartCoroutine(FadeAndLoad(other));
        }
    }

    IEnumerator FadeAndLoad(Collider2D player)
    {
        SaveManager.GetInstance().UpdateSavedDuration(Mathf.RoundToInt(Timer.GetInstance().CountdownTime));

        if((string.Equals("Island", sceneToLoad)) && (string.Equals(SceneManager.GetActiveScene().name, "Forest")))
        {
            SaveManager.GetInstance().ChangeIsLevelStartTransitionOnValue(true);
            SaveManager.GetInstance().UpdateSavedDuration(Mathf.RoundToInt(901));
        }


        // Menonaktifkan kontrol pergerakan pemain
        player.GetComponent<PlayerMovement>().enabled = false;

        // Memutar trigger "Fade" pada Animator yang berada dalam GameObject yang memiliki tag "Transition"
        GameObject.FindWithTag("Transition").GetComponentInParent<Animator>().SetTrigger("Fade");

        // Menunggu selama fadeWait
        yield return new WaitForSeconds(fadeWait);

        // Memuat adegan baru secara asinkron
        AsyncOperation loadNewScene = SceneManager.LoadSceneAsync(sceneToLoad);

        //// Menunggu hingga proses pengisian adegan baru selesai
        //while (!loadNewScene.isDone)
        //    yield return null;
    }
}
