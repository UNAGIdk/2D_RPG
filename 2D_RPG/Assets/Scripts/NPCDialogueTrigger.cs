using UnityEngine;

public class NPCDialogueTrigger : Collidable
{
    public NPCTextPerson NPC;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player1" || coll.name == "Player2(Clone)")
            NPC.OnCollisionWithTrigger();
    }
}
