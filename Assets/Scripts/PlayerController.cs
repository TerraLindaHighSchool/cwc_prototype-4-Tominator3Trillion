using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5.0f;
    private bool hasPowerup;
    private float powerupStrength = 15.0f;

    public GameObject powerupIndicator;

    private GameObject focalPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

   
    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 force = new Vector3(horizontalInput, 0, verticalInput);

        playerRb.AddForce(force.normalized * speed);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (transform.position.y < -10)
        {
            //Quit the game
            Application.Quit();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Powerup")) {
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
            Destroy(other.gameObject);
        }
    }

    IEnumerator PowerupCountdownRoutine() {
        yield return new WaitForSeconds(7);
        hasPowerup=false;
        powerupIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup) {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            
            Debug.Log("Player collided with enemy and has powerup");
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    

}
