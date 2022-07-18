using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Portal : Collidable
{
    public string sceneName; //�������� ����, �� ������� �� ����� �������������
    
    //public Sprite teleportButtonSprite;
    public GameObject teleportButtonPrefab;
    //public GameObject teleportTextPrefab;
    public Transform portalTransform;

    private GameObject teleportButton;
    //private GameObject teleportText;
    private bool teleportHintShowing = false;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            if (teleportHintShowing == false)
            {
                teleportButton = Instantiate(teleportButtonPrefab, portalTransform);
                //teleportText = Instantiate(teleportTextPrefab, portalTransform);

                //teleportButton.GetComponent<Image>().sprite = teleportButtonSprite;
                //teleportText.GetComponent<Text>().text = "to go into " + sceneName;

                teleportHintShowing = true;
            }

            if (Input.GetKeyDown(KeyCode.F) == true)
            {
                ClearPortalText();
                ToDungeon();
            }
        }    
    }

    protected override void Update()
    {
        base.Update();
        if(teleportHintShowing == true)
        {
            ClearPortalText();
        }
    }

    private void ToDungeon()
    {
        //��������������� ������ � dungeon
        GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)]; //������� ��� ������� �����, ��� ��� ����� ������ ����
        SceneManager.LoadScene(sceneName);
    }

    private void ClearPortalText()
    {
        Destroy(teleportButton);
        //Destroy(teleportText);
        teleportHintShowing = false;
    }
}
