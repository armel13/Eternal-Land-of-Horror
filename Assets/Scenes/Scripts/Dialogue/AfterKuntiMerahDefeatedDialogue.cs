using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterKuntiMerahDefeatedDialogue : Singleton<AfterKuntiMerahDefeatedDialogue>
{
    [Header("Ink Json")]
    [SerializeField] private TextAsset inkJSON;

    public void StartAfterKuntiMerahDefeatedDialogue()
    {
        DialogueManager.GetInstance()?.EnterDialogueMode(inkJSON);
        StartCoroutine(WaitForDialogueToEnd());
    }

    private IEnumerator WaitForDialogueToEnd()
    {
        // Tunggu sampai dialog selesai
        yield return new WaitUntil(() => !DialogueManager.GetInstance().dialogueIsPlaying);
        Debug.Log("DIalogue After Fighting Kunti Merah");
        UIManager.instance.BossDefeated();
        gameObject.SetActive(false);
    }
}
