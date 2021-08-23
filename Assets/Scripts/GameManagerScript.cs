using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private bool previousIsColorSwitch;
    public List<GameObject> levelContentList = new List<GameObject>();
    private List<GameObject> obstacleList = new List<GameObject>();
    [SerializeField] private GameObject player;
    private Camera myCamera;
    private float contentSpawnPos;
    public int spawnAmount;

    public float obstacleSpawnDistance = 3;
    public GameObject playerPF;
    public GameObject colorSwitchPF;
    public GameObject starPF;
    public GameObject hoopObstPF;
    public GameObject wheelObstPF;
    public GameObject wallObstPF;
    public Text pointsCounterText;
    public Text finalScoreText;

    private void Start()
    {
        myCamera = Camera.main;
        pointsCounterText = GetComponentInChildren<Text>();
        previousIsColorSwitch = false;
        spawnAmount = 8; //amount of level elements active at any time

        obstacleList.Add(hoopObstPF);
        obstacleList.Add(wheelObstPF);
        obstacleList.Add(wallObstPF);

        InstantiateLevel();
    }

    private void Update()
    {
        if (player == null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            InstantiateLevel();
        }
        else if (levelContentList[0].transform.position.y < Camera.main.transform.GetChild(0).transform.position.y)
        {
            GameObject destr = levelContentList[0];
            levelContentList.RemoveAt(0);
            Destroy(destr);
            InstantiateObject();
        }
    }

    public void ScorePoint()
    {
        points++;

        if (pointsCounterText != null)
        {
            pointsCounterText.text = $"Points: {points}";
        }
    }

    public void InstantiateObject()
    {
        //colorswitch, star or obstacle
        //obstacle might have something else inside

        GameObject instantiatedObject = null;
        switch (Random.Range(0, 2))
        {
            case 0: //collectable
                {
                    if (Random.Range(0, 2) == 0 && previousIsColorSwitch == false) //colorswitch
                    {
                        //Add half of new objects's height to spawn position
                        contentSpawnPos = contentSpawnPos + colorSwitchPF.GetComponent<SpriteRenderer>().bounds.extents.y;
                        instantiatedObject = Instantiate(colorSwitchPF, new Vector3(0, contentSpawnPos, 1), Quaternion.identity);
                        previousIsColorSwitch = true;
                    }
                    else //star
                    {
                        contentSpawnPos = contentSpawnPos + starPF.GetComponent<SpriteRenderer>().bounds.extents.y;
                        instantiatedObject = Instantiate(starPF, new Vector3(0, contentSpawnPos, 1), Quaternion.identity);
                    }

                    //Update spawn point: new object's y position + half of its height + distance to next object
                    contentSpawnPos = instantiatedObject.transform.position.y + instantiatedObject.GetComponent<SpriteRenderer>().bounds.extents.y + 1;
                    break;
                }
            case 1: //obstacle
                {
                    instantiatedObject = obstacleList[Random.Range(0, obstacleList.Count)];

                    //Obstacles have multiple child objects which means the bounds need to be combined
                    Bounds bounds = new Bounds(instantiatedObject.transform.position, Vector3.zero);
                    foreach (SpriteRenderer renderer in instantiatedObject.GetComponentsInChildren<SpriteRenderer>())
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }

                    float instPosX = 0;
                    if (instantiatedObject == wheelObstPF) //Wheel obstacle needs to be moved slightly to one side
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                instPosX = 0.8f;
                                break;

                            case 1:
                                instPosX = -0.8f;
                                break;

                            default:
                                //ERROR
                                break;
                        }
                    }

                    contentSpawnPos = contentSpawnPos + (bounds.extents.y);

                    if (instantiatedObject == hoopObstPF) //Hoop obstacles might have collectable items inside
                    {
                        switch (Random.Range(0, 4))
                        {
                            case 0:
                            case 1:
                                break; //50% chance of no item

                            case 2: //Colorswitch
                                levelContentList.Add(Instantiate(colorSwitchPF, new Vector3(0, contentSpawnPos, 1), Quaternion.identity));
                                break;

                            case 3: //Star
                                levelContentList.Add(Instantiate(starPF, new Vector3(0, contentSpawnPos, 1), Quaternion.identity));
                                break;

                            default:
                                //ERROR
                                break;
                        }
                    }

                    instantiatedObject = Instantiate(instantiatedObject, new Vector3(instPosX, contentSpawnPos, 1), Quaternion.identity);
                    previousIsColorSwitch = false;

                    //Update spawn point: new object's y position + half of its height + distance to next object
                    contentSpawnPos = instantiatedObject.transform.position.y + bounds.extents.y + obstacleSpawnDistance;
                    break;
                }
            default:
                {
                    //ERROR
                    break;
                }
        }

        if (instantiatedObject != null)
        {
            levelContentList.Add(instantiatedObject);
        }
    }

    private void InstantiateLevel()
    {
        points = 0;
        if (pointsCounterText != null)
        {
            pointsCounterText.text = $"Points: {points}";
        }
        pointsCounterText.transform.parent.gameObject.SetActive(true);
        finalScoreText.transform.parent.gameObject.SetActive(false);

        foreach (GameObject obj in levelContentList)
        {
            Destroy(obj);
        }
        levelContentList.Clear();

        contentSpawnPos = 3; //initialize spawn position
        for (int i = 0; i < spawnAmount; i++)
        {
            InstantiateObject();
        }
        player = Instantiate(playerPF, new Vector3(0, 0, 1), Quaternion.identity);
        player.GetComponentInChildren<PlayerScript>().gameManager = this;
        myCamera.GetComponent<CameraMovementScript>().player = player.GetComponentInChildren<PlayerScript>().gameObject.transform;
        myCamera.transform.position = new Vector3(0, 0, 0);
    }

    public void GameOver()
    {
        finalScoreText.text = $"Final score\n{points}";
        pointsCounterText.transform.parent.gameObject.SetActive(false);
        finalScoreText.transform.parent.gameObject.SetActive(true);
    }
}