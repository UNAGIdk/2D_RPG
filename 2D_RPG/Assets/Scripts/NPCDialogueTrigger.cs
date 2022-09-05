using UnityEngine;

public class NPCDialogueTrigger : Collidable
{
    public NPCTextPerson NPC;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
            NPC.OnCollisionWithTrigger();
    }
}
