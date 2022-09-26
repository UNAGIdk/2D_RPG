using UnityEngine;

public class NPCDialogueTrigger : Collidable
{
    public NPCTextPerson NPC;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player1" || coll.name == "Player2(Clone)")
        {
            if (PlayerPrefs.GetString("LevelsPassed") == "0" || PlayerPrefs.GetString("LevelsPassed") == "")
                NPC.OnCollisionWithTrigger();
        }
    }
}
