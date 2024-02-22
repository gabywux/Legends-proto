using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHit : MonoBehaviour
{
    [Tooltip("Determines when the player is taking damage.")]
    public bool hurt = false;

    private bool slipping = false;
    private PlayerMovement playerMovementScript;
    private Rigidbody rb;
    private Transform enemy;
    
    public float maxHealth = 100f;
    private float currentHealth;

    // UI elements
    public Text healthText;

    private void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    private void FixedUpdate()
    {
        // stops the player from running up the slopes and skipping platforms
        if (slipping == true)
        {
            transform.Translate(Vector3.back * 20 * Time.deltaTime, Space.World);
            playerMovementScript.playerStats.canMove = false;
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (hurt == false)
        {
            if (other.gameObject.tag == "Enemy")
            {
                enemy = other.gameObject.transform;
                rb.AddForce(enemy.forward * 1000);
                rb.AddForce(transform.up * 500);
                TakeDamage(30f);
            }
            if (other.gameObject.tag == "Trap")
            {
                rb.AddForce(transform.forward * -1000);
                rb.AddForce(transform.up * 500);
                TakeDamage(30f);
            }
        }
        if (other.gameObject.layer == 9)
        {
            slipping = true;
        }
        if (other.gameObject.layer != 9)
        {
            if (slipping == true)
            {
                slipping = false;
                playerMovementScript.playerStats.canMove = true;
            }
        }
    }
    private void TakeDamage(float damageAmount)
    {
        hurt = true;
        playerMovementScript.playerStats.canMove = false;
        playerMovementScript.soundManager.PlayHitSound();
        
        currentHealth -= damageAmount;
        UpdateHealthUI();

        // Check if the player is still alive
        if (currentHealth <= 0)
        {
            // Handle player death (e.g., restart level or game over screen)
            Debug.Log("Player is dead!");
        }
        
        StartCoroutine("Recover");
    }
    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.75f);
        hurt = false;
        playerMovementScript.playerStats.canMove = true;
    }
    
    public void UpdateHealthUI()
    {
        // Update the health text on the screen
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth.ToString();
        }
    }
    public void IncreaseHealth(float amount)
    {
        // Increase the player's health
        currentHealth += amount;

        // Clamp the health to the maximum value
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Update the UI
        UpdateHealthUI();
    }
}
