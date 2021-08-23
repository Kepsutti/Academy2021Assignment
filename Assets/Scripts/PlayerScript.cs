using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private AudioSource myAudioSource;
    private Vector2 impulseVector;
    private Rigidbody2D myRigidbody;
    private Color[] colorArray;

    public GameManagerScript gameManager;
    public GameObject deathParticleEffectPF;
    public GameObject starParticleEffectPF;
    public AudioClip starAudio;
    public AudioClip jumpAudio;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();

        impulseVector = new Vector2(0.0f, 5.0f);

        colorArray = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.magenta
        };

        int colorIndex = Random.Range(0, colorArray.Length);
        GetComponent<SpriteRenderer>().material.color = colorArray[colorIndex];
        gameObject.layer = colorIndex + 6;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.AddForce(impulseVector, ForceMode2D.Impulse);
            myAudioSource.PlayOneShot(jumpAudio);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "GameOver":
                gameManager.GameOver();
                Instantiate(deathParticleEffectPF, transform.position, Quaternion.identity);
                Destroy(gameObject.transform.parent.gameObject);
                break;

            case "ColorSwitch":
                Color currentColor = GetComponent<SpriteRenderer>().material.color;
                Color newColor = currentColor;
                int colorIndex = 0;
                while (newColor == currentColor)
                {
                    colorIndex = Random.Range(0, colorArray.Length);
                    newColor = colorArray[colorIndex];
                }
                GetComponent<SpriteRenderer>().material.color = newColor;

                //Change the layer of the player gameobject based on the new color
                gameObject.layer = colorIndex + 6;

                gameManager.levelContentList.Remove(collision.gameObject);
                Destroy(collision.gameObject);
                myAudioSource.PlayOneShot(starAudio);
                if (gameManager.levelContentList.Count < gameManager.spawnAmount)
                {
                    gameManager.InstantiateObject();
                }
                break;

            case "Star":
                gameManager.ScorePoint();
                Instantiate(starParticleEffectPF, collision.transform.position, Quaternion.identity);
                gameManager.levelContentList.Remove(collision.gameObject);
                Destroy(collision.gameObject);
                myAudioSource.PlayOneShot(starAudio);
                if (gameManager.levelContentList.Count < gameManager.spawnAmount)
                {
                    gameManager.InstantiateObject();
                }
                break;

            default:
                //ERROR
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //This function removes the starting platform after the first jump
        Destroy(collision.gameObject);
    }
}