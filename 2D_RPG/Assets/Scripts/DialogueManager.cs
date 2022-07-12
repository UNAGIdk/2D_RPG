using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject container; //dialogue manager
    public Transform containerTransform; //dialogue manager
    public GameObject dialogueWindowPrefab;
    public GameObject DialogueCharacterSpritePrefab;
    

    private GameObject dialogue;
    private GameObject characterSpriteWindow;


    private bool dialogueRunning = false;
    private int pagesCounter = 0;

    private void Update()
    {
        if(dialogueRunning == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) == true || Input.GetKeyDown(KeyCode.Space) == true)
            {
                pagesCounter += 1;
            }
        }
    }

    public void Show(string[] textPages, Sprite characterSprite)
    {
        if(dialogueRunning == false)
        {
            dialogue = Instantiate(dialogueWindowPrefab, containerTransform);
            characterSpriteWindow = Instantiate(DialogueCharacterSpritePrefab, containerTransform);
            dialogueRunning = true;
        }

        characterSpriteWindow.GetComponentInChildren<Image>().sprite = characterSprite;

        dialogue.GetComponentInChildren<Text>().text = textPages[pagesCounter];
        
        if(pagesCounter > textPages.Length)
        {
            ClearDialogue();
        }
    }

    public void UpdateDialogue(string[] textPages)
    {

    }

    public void ClearDialogue()
    {
        Destroy(dialogue);
        Destroy(characterSpriteWindow);
        dialogueRunning = false;
    }
}