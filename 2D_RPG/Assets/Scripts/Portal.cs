using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Portal : Collidable
{
    public string sceneName; //�������� ����, �� ������� �� ����� �������������
    
    public Sprite teleportButtonSprite;
    public GameObject teleportButtonPrefab;
    //public GameObject teleportTextPrefab;
    public Transform portalTransform;

    private GameObject teleportButton;
    //private GameObject teleportText;
    public bool teleportHintShowing;

    protected override void Start()
    {
        base.Start();
        teleportHintShowing = false;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            Debug.Log("collided portal");
            if (teleportHintShowing == false)
            {
                teleportButton = Instantiate(teleportButtonPrefab, portalTransform);
                teleportButton.transform.localPosition = new Vector3(0, 4.53f, 0);
                //teleportText = Instantiate(teleportTextPrefab, portalTransform);

                teleportButton.GetComponent<SpriteRenderer>().sprite = teleportButtonSprite;
                //teleportText.GetComponent<Text>().text = "to go into " + sceneName;

                teleportHintShowing = true;
            }

            if (Input.GetKeyDown(KeyCode.F) == true)
            {
                ClearPortalText();
                ToDungeon();
            }
        }
        else
        {
            ClearPortalText();
        }
    }

    private void ToDungeon()
    {
        //��������������� ������ � dungeon
        GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)]; //������� ��� ������� �����, ��� ��� ����� ������ ����
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void ClearPortalText()
    {
        Destroy(teleportButton);
        //Destroy(teleportText);
        teleportHintShowing = false;
    }
}
