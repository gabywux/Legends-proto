using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score;

    void Start()
    {
        // resets the score to 0 at the start of the game
        score = 0;
    }
    public static void IncrementScore(int amount)
    {
        score += amount;

        // Check if the player collected enough coins to gain +1 HP
        if (score % 100 == 10)  // Assuming 10 coins for +1 HP (adjust as needed)
        {
            // Assuming there's only one player in the scene, you might need to adjust this accordingly
            GetHit getHitScript = FindObjectOfType<GetHit>();
            if (getHitScript != null)
            {
                getHitScript.IncreaseHealth(1f);
            }
        }
    }
}
