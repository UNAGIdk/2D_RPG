using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAny : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSound; //звук при наведении на кнопку
    public AudioClip clickSound; //звук при клике

    public void HoverSound()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(hoverSound);
    }

    public void ClickSound()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clickSound);
    }
}