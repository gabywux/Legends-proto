using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorEnemy : MonoBehaviour
{
    public Stats enemyStats;
    public Transform sight;
    public GameObject enemyExplosionParticles;
    public Rigidbody rb;

    private GameObject player;
    private bool slipping = false;

    [System.Serializable]
    public struct Stats
    {
        public float walkSpeed;
        public float rotateSpeed;
        public float chaseSpeed;
        public bool idle;
        public float explodeDist;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // changes the enemy's behavior: pacing in circles or chasing the player
        if (enemyStats.idle == true)
        {
        }
        else if (enemyStats.idle == false)
        {
            ChasePlayer();
        }

        // stops enemy from following player up the inaccessible slopes
        if (slipping == true)
        {
            transform.Translate(Vector3.back * 20 * Time.deltaTime, Space.World);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        slipping = other.gameObject.layer == 9;
    }


    private void OnTriggerEnter(Collider other)
    {
        //start chasing if the player gets close enough
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            enemyStats.idle = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //stop chasing if the player gets far enough away
        if (other.CompareTag("Player"))
        {
            enemyStats.idle = true;
        }
    }

    private IEnumerator Explode()
    {
        GameObject particles = Instantiate(enemyExplosionParticles, transform.position, new Quaternion());
        yield return new WaitForSeconds(0.2f);
        Destroy(transform.parent.gameObject);
    }

    public void ChasePlayer()
    {
        //Chase the player
        sight.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(sight);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position,
            Time.deltaTime * enemyStats.chaseSpeed);

        //Explode if we get within the enemyStats.explodeDist
        if (Vector3.Distance(transform.position, player.transform.position) < enemyStats.explodeDist)
        {
            StartCoroutine("Explode");
            enemyStats.idle = true;
        }
    }
}