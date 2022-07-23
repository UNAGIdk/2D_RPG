using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    private AudioSource crateAudioSource;
    public AudioClip crateDamageAudioClip;
    public AudioClip crateDestroyAudioClip;

    private void Start()
    {
        crateAudioSource = FindObjectOfType<AudioManager>().GetComponentInChildren<AudioSource>();
    }

    protected override void Death()
    {
        crateAudioSource.PlayOneShot(crateDestroyAudioClip);
        Destroy(gameObject);
    }

    protected override void RecieveDamage(Damage damage)
    {
        //crateAudioSource.PlayOneShot(crateDamageAudioClip);
        base.RecieveDamage(damage);
    }
}