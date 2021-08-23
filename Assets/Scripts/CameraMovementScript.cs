using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        if (player != null)
        {
            float playerHeight = player.position.y;

            if (playerHeight > transform.position.y)
            {
                transform.position = new Vector2(transform.position.x, playerHeight);
            }
        }
    }
}