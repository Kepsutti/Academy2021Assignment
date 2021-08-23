using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObstacleScript : MonoBehaviour
{
    private int moveDirection;
    private int segmentColor;
    private int instantiationDirection;
    private float instantiationPosition;
    private float disappearDistanceX;
    public List<GameObject> segmentList = new List<GameObject>();
    public float speed = 0.005f;

    public GameObject segmentPF;

    private void Start()
    {
        segmentColor = 0;
        instantiationDirection = 1;
        instantiationPosition = -5f;
        disappearDistanceX = segmentPF.GetComponent<SpriteRenderer>().bounds.extents.x * 3;
        if (Random.Range(0, 2) == 0)
        {
            instantiationDirection = -1;
            instantiationPosition = 5f;
        }

        for (int i = 0; i < 6; i++)
        {
            InstantiateSegment();
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.left * speed * instantiationDirection);

        if (instantiationDirection == 1 && segmentList[0].transform.position.x < Camera.main.transform.position.x - disappearDistanceX)
        {
            GameObject destr = segmentList[0];
            segmentList.RemoveAt(0);
            Destroy(destr);
            InstantiateSegment();
        }
        else if (instantiationDirection == -1 && segmentList[0].transform.position.x > Camera.main.transform.position.x + disappearDistanceX)
        {
            GameObject destr = segmentList[0];
            segmentList.RemoveAt(0);
            Destroy(destr);
            InstantiateSegment();
        }
    }

    private void InstantiateSegment()
    {
        if (segmentList.Count != 0)
        {
            instantiationPosition = segmentList[segmentList.Count - 1].transform.position.x + (segmentPF.GetComponent<SpriteRenderer>().bounds.extents.x * instantiationDirection);
        }
        GameObject newSegment = Instantiate(segmentPF, new Vector3(instantiationPosition, transform.position.y, 1), Quaternion.AngleAxis(90, Vector3.forward), this.transform);
        switch (segmentColor)
        {
            case 0:
                newSegment.GetComponent<SpriteRenderer>().color = Color.red;
                newSegment.layer = 6;
                break;

            case 1:
                newSegment.GetComponent<SpriteRenderer>().color = Color.green;
                newSegment.layer = 7;
                break;

            case 2:
                newSegment.GetComponent<SpriteRenderer>().color = Color.blue;
                newSegment.layer = 8;
                break;

            case 3:
                newSegment.GetComponent<SpriteRenderer>().color = Color.magenta;
                newSegment.layer = 9;
                break;
        }
        segmentList.Add(newSegment);

        segmentColor++;
        if (segmentColor > 3)
        {
            segmentColor = 0;
        }
    }
}