using UnityEngine;
using UnityEngine.SceneManagement;

//��������� ��� � �� �������, ���� ��� �������� ��� ������������ ����


public class Portal : Collidable
{
    public string[] sceneNames; //������ � ���������� ����, �� ������� �� ����� �������������
    public Animator toDungeonWindow; //���� ��������� ���� �������� �� �������

    protected override void OnCollide(Collider2D coll)
    {
        /*if(coll.name == "Player")
        {
            if(triggerCanNowBeSet)
            {
                toDungeonWindow.SetTrigger("Show");
                GameManager.instance.ProhibitPlayerMoving();
            }
        }*/
        if (coll.name == "Player") //���������� ���������
            ToRandomDungeon();
    }

    private void ToRandomDungeon()
    {
        //��������������� ������ � ��������� dungeon
        GameManager.instance.SaveState();
        string sceneName = sceneNames[Random.Range(0, sceneNames.Length)]; //��������� ���������� sceneName ��������, ������ ���������� �������� �� ������� sceneNames
        SceneManager.LoadScene(sceneName);
    }

    public void OnYesButtonClick()
    {
        ToRandomDungeon();
        GameManager.instance.AllowPlayerMoving();
        toDungeonWindow.SetTrigger("Hide");
    }

    public void OnNoButtonClick()
    {
        //�������� ������ ������� ���� ���� �� ����� no
        GameManager.instance.AllowPlayerMoving();
        GameManager.instance.player.transform.localPosition = new Vector3(GameManager.instance.player.transform.localPosition.x, GameManager.instance.player.transform.localPosition.y - 0.24f, 0);
        toDungeonWindow.SetTrigger("Hide");
    }
}
