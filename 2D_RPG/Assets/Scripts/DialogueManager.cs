using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject container; //dialogue manager
    public GameObject dialogueWindowPrefab;
    public GameObject characterSpriteWindowPrefab;

    private GameObject dialogue;
    private GameObject characterSpriteWindow;


    private bool objectsCreated = false;


    public void Show(string[] textPages, Sprite characterSprite)
    {
        if(objectsCreated == false)
        {
            dialogue = Instantiate(dialogueWindowPrefab);
            characterSpriteWindow = Instantiate(characterSpriteWindowPrefab);
            objectsCreated = true;
        }

        dialogue.transform.position = GameManager.instance.player.transform.position; //позиция где отрисовывается новый dialogue
        characterSpriteWindow.transform.position = dialogue.transform.position -= new Vector3(0.3f, 0, 0); //позиция где отрисовывается новый characterSpriteWindow
        characterSpriteWindow.GetComponentInChildren<Image>().sprite = characterSprite;



        int pagesCounter = 0;
        dialogue.GetComponentInChildren<Text>().text = textPages[pagesCounter];
        /*while (pagesCounter <= textPages.Length)
        {
            dialogue.GetComponentInChildren<Text>().text = textPages[pagesCounter];
            if (Input.GetKeyDown(KeyCode.Mouse0) == true || Input.GetKeyDown(KeyCode.Space) == true)
            {
                pagesCounter += 1;
            }
        }
        Destroy(dialogue);
        Destroy(characterSpriteWindow);
        objectsCreated = false;*/
    }
}
