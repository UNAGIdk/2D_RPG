using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    private AudioSource crateAudioSource;
    public AudioClip crateDamageAudioClip;
    public AudioClip crateDestroyAudioClip;

    //ParticleSystem 
    public Object enemyExplosionPrefabGO;
    public ParticleSystemRenderer enemyExplosionPrefabPSR;
    public Material destroyMaterial;



    private void Start()
    {
        crateAudioSource = FindObjectOfType<AudioManager>().GetComponentInChildren<AudioSource>();
    }

    protected override void Death()
    {
        enemyExplosionPrefabPSR.material = destroyMaterial;
        GameObject enemyExplosion = (GameObject)Instantiate(enemyExplosionPrefabGO);
        enemyExplosion.transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
        crateAudioSource.PlayOneShot(crateDestroyAudioClip);
        Destroy(gameObject);
    }

    protected override void RecieveDamage(Damage damage)
    {
        //crateAudioSource.PlayOneShot(crateDamageAudioClip);
        base.RecieveDamage(damage);
    }
}