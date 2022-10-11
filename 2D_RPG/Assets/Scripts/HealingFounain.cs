using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFounain : Collidable
{
    public int healingAmount = 1;

    private float healCooldown = 1.0f;
    private float lastHeal;

    public AudioSource healingFountainAudioSource;
    public AudioClip healingAudioClip;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player1" || coll.name == "Player2(Clone)")
        {
            Debug.Log("condition in healingfountain works now");
            if (Time.time - lastHeal > healCooldown)
            {
                lastHeal = Time.time;
                GameManager.instance.player.Heal(healingAmount);
                if (GameManager.instance.player.hitpoint != GameManager.instance.player.maxHitpoint)
                {
                    healingFountainAudioSource.pitch = Random.Range(0.8f, 1.2f);
                    healingFountainAudioSource.PlayOneShot(healingAudioClip);
                }
            }
        }
    }
}
