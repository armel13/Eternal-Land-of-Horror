using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterShopDialogue : Singleton<AfterShopDialogue>
{
    
    [Header("Ink Json")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private GameObject shopCollider;

    public void StartAfterShopDialogue()
    {
        DialogueManager.GetInstance()?.EnterDialogueMode(inkJSON);
        shopCollider.SetActive(false);
        StartCoroutine(WaitForDialogueToEnd());
    }

    private IEnumerator WaitForDialogueToEnd()
    {
        // Tunggu sampai dialog selesai
        yield return new WaitUntil(() => !DialogueManager.GetInstance().dialogueIsPlaying);
        Debug.Log("DIalogue After Shop Done");
        shopCollider.SetActive(true);
        gameObject.SetActive(false);
    }
}
