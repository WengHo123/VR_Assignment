using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI instructionText;
    public GameObject instructionPanel;
    public XRGrabInteractable swordInteractable;
    private bool isSwordInteracted = false;
    public EnemySword enemySword;
    public bool IsSwordInteracted {  get { return isSwordInteracted;} }

    void Start()
    {
        ShowInstruction("Pick up the sword using the Grab button.");
        StartCoroutine(HideUIAfterDelay(5.0f));
        swordInteractable.selectEntered.AddListener(OnSwordPickedUp);
    }

    void OnSwordPickedUp(SelectEnterEventArgs args)
    {
        if (!isSwordInteracted)
        {
            isSwordInteracted = true;
            StartCoroutine(ShowInstructionsSequence());
        }
    }

    IEnumerator ShowInstructionsSequence()
    {
        ShowInstruction("You picked up the sword!");
        yield return new WaitForSeconds(2.0f);

        ShowInstruction("Now kill the enemy!");
        yield return new WaitForSeconds(2.0f);

        ShowInstruction("You can use your sword to block the enemy attack");
        yield return new WaitForSeconds(2.0f);
        HideInstruction();

        StartCoroutine(CheckBlockCondition());
    }

    IEnumerator CheckBlockCondition()
    {
        while (enemySword.IsBlock == false)
        {
            yield return null;
        }

        ShowInstruction("Good job, you blocked an enemy attack");
        yield return new WaitForSeconds(2.0f);
        HideInstruction();
    }

    void ShowInstruction(string message)
    {
        instructionText.text = message;
        instructionText.gameObject.SetActive(true);
        instructionPanel.SetActive(true);
    }

    void HideInstruction()
    {
        instructionText.gameObject.SetActive(false);
        instructionPanel.SetActive(false);
    }

    private IEnumerator HideUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        instructionText.gameObject.SetActive(false);
        instructionPanel.SetActive(false);
    }
}
