using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    public GameObject talkPrompt;
    public GameObject nextButton;
    public GameObject reticle;

    public CameraController playerCam;
    public PlayerMovement playerMovement;

    [Header("Variables")]
    public string[] dialogue;
    private int index;

    public float typeSpeed;
    private bool playerIsClose;

    private bool talkPromptNotVisible;

    [Header("Interaction Detection")]
    public LayerMask whatIsPlayer;
    public float detectRange;

    private void Update()
    {
        playerIsClose = Physics.CheckSphere(transform.position, detectRange, whatIsPlayer);

        if (playerIsClose && !talkPromptNotVisible)
        {
            talkPrompt.SetActive(true);
        }
        else
        {
            talkPrompt.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                UnfreezePlayer();
                ResetText();
            }

            else
            {
                talkPromptNotVisible = true;

                reticle.SetActive(false);

                FreezePlayer();

                dialoguePanel.SetActive(true);
                StartCoroutine(TypeLine());
            }
        }

        if (dialogueText.text == dialogue[index])
        {
            nextButton.SetActive(true);
        }
    }

    public void ResetText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    private IEnumerator TypeLine()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public void NextLine()
    {
        nextButton.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            talkPromptNotVisible = false;
            reticle.SetActive(true);

            UnfreezePlayer();
            ResetText();
        }
    }

    private void FreezePlayer()
    {
        playerCam.enabled = false;
        playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void UnfreezePlayer()
    {
        playerCam.enabled = true;
        playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
