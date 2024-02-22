using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoins : MonoBehaviour
{
    [Tooltip("The particles that appear after the player collects a coin.")]
    public GameObject coinParticles;

    private GetHit getHitScript;


    PlayerMovement playerMovementScript;

    void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
        getHitScript = GetComponent<GetHit>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerMovementScript = other.GetComponent<PlayerMovement>();
            playerMovementScript.soundManager.PlayCoinSound();
            ScoreManager.score += 10;

            // Find the GetHit script in the scene
            GetHit getHitScript = FindObjectOfType<GetHit>();
            if (getHitScript != null)
            {
                // Assuming 10 coins for +1 HP (adjust as needed)
                if (ScoreManager.score % 100 == 0)
                {
                    getHitScript.IncreaseHealth(1);
                }
            }

            GameObject particles = Instantiate(coinParticles, transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }
}