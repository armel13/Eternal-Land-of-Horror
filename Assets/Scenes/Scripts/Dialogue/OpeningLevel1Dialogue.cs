using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningLevel1Dialogue : Singleton<OpeningLevel1Dialogue>
{
    [Header("Ink Json")]
    [SerializeField] private TextAsset inkJSON;

    public void StartLevel1Dialogue()
    {
        Player.instance.SavePlayer();
        DialogueManager.GetInstance()?.EnterDialogueMode(inkJSON);
        StartCoroutine(WaitForDialogueToEnd());
    }

    private IEnumerator WaitForDialogueToEnd()
    {
        // Tunggu sampai dialog selesai
        yield return new WaitUntil(() => !DialogueManager.GetInstance().dialogueIsPlaying);
        Debug.Log("DIalogue Awal Done");
        if (Timer.GetInstance() != null) Timer.GetInstance().StartTimer();
        if (InfoManager.Instance != null) InfoManager.Instance.hasSeenOpeningDialogue = true;
    }
}
