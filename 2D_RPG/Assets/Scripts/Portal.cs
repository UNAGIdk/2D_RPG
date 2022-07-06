using UnityEngine;
using UnityEngine.SceneManagement;

//ПОФИКСИТЬ ТАК И НЕ УДАЛОСЬ, ПОКА ЧТО ОСТАВЛЯЮ БЕЗ ВСПЛЫВАЮЩЕГО ОКНА


public class Portal : Collidable
{
    public string[] sceneNames; //массив с названиями сцен, на которые мы хотим переключаться
    public Animator toDungeonWindow; //поле аниматора меню перехода на локацию

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
        if (coll.name == "Player") //заменяющая структура
            ToRandomDungeon();
    }

    private void ToRandomDungeon()
    {
        //телепортировать игрока в рандомный dungeon
        GameManager.instance.SaveState();
        string sceneName = sceneNames[Random.Range(0, sceneNames.Length)]; //присвоить переменной sceneName значение, равное рандомному элементу из массива sceneNames
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
        //сместить игрока немного вниз если он нажал no
        GameManager.instance.AllowPlayerMoving();
        GameManager.instance.player.transform.localPosition = new Vector3(GameManager.instance.player.transform.localPosition.x, GameManager.instance.player.transform.localPosition.y - 0.24f, 0);
        toDungeonWindow.SetTrigger("Hide");
    }
}
