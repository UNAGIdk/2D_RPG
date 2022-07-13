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


    public bool dialogueRunning = false;

    //(Input.GetKeyDown(KeyCode.Mouse0) == true || Input.GetKeyDown(KeyCode.Space) == true)

    public void ShowPage(string textPage, Sprite characterSprite)
    {
        if(dialogueRunning == false)
        {
            dialogue = Instantiate(dialogueWindowPrefab, containerTransform);
            characterSpriteWindow = Instantiate(DialogueCharacterSpritePrefab, containerTransform);
            dialogueRunning = true;
            characterSpriteWindow.GetComponentInChildren<Image>().sprite = characterSprite;
            dialogue.GetComponentInChildren<Text>().text = textPage;
        }
        else
        {
            characterSpriteWindow.GetComponentInChildren<Image>().sprite = characterSprite;
            dialogue.GetComponentInChildren<Text>().text = textPage;
        }
    }

    public void ClearDialogue()
    {
        Destroy(dialogue);
        Destroy(characterSpriteWindow);
        dialogueRunning = false;
    }
}