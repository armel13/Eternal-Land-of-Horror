using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{

    public Button loadButton;
    public GameObject optionPanel;
    public GameObject creditPanel;
    public GameObject guidePanel;
    private InfoManager infoManager;

    public AudioSource backgroundMusic; // Tambahkan ini
    public AudioClip backgroundMusicClip; // Tambahkan ini

    void Start()
    {
   
        if (InfoManager.Instance != null)
        {

            InfoManager.Instance.NewGameStats();
        }
        else
        {
    
            Debug.LogError("InfoManager not found in the scene.");
        }

        string savePath = Application.persistentDataPath + "/player.json";
        if (File.Exists(savePath))
        {

            loadButton.interactable = true;
        }
        else
        {
  
            loadButton.interactable = false;
        }

        // Mainkan background music jika tidak sedang bermain
        if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.clip = backgroundMusicClip;
            backgroundMusic.Play();
        }
    }

    private void Update()
    {
        if (SaveManager.GetInstance() != null)
        {
            backgroundMusic.volume = SaveManager.GetInstance().SavedVolumeValue;
        }
    }


    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnNewGameButtonClicked()
    {
        InfoManager.Instance.NewListAfterNewGame();
        InfoManager.Instance.NewGameStats();
        SaveManager.GetInstance().UpdateSavedDuration(360);

        SceneManager.LoadScene("Prolog"); //Disabled for testing
        //SceneManager.LoadScene("Forest"); 
    }


    public void OnLoadGameButtonClicked()
    {
        
        InfoManager.Instance.LoadStats();
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Quit Game");
    }

    public void ShowOptions()
    {
        // Aktifkan panel opsi
        if (optionPanel != null)
        {
            optionPanel.SetActive(true);
        }
    }

    public void HideOptions()
    {
        // Nonaktifkan panel opsi
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
        }
    }

    public void GuideOpen()
    {
        if (guidePanel != null)
        {
            guidePanel.SetActive(true);
        }
    }

    public void HideGuide()
    {
        if (guidePanel != null)
        {
            guidePanel.SetActive(false);
        }
    }

    public void OpenCredit()
    {
        creditPanel.SetActive(true);
    }

    public void HideCredit()
    {
        creditPanel.SetActive(false);
    }
}
