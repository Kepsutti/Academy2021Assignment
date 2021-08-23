using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObstacleScript : MonoBehaviour
{
    public int rotationSpeed = 10;

    private void Start()
    {
        //Randomize rotation direction
        if (Random.Range(0, 2) == 0)
        {
            rotationSpeed = rotationSpeed * -1;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
    }
}