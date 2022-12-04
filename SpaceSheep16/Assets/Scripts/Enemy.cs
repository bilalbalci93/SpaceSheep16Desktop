using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] public GameObject enemyBulletPrefab;
    [SerializeField] float enemyBulletSpeed = 2f;

    [Header("Death VFX")]
    [SerializeField] private bool _showVFX = true;
    [SerializeField] public GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;

    [Header("Enemy Death SFX")]
    [SerializeField] public AudioClip deathSFX;
    [SerializeField] [Range(0,1)] float deathVolume = 0.75f;

    [Header("Enemy Shoot SFX")]
    [SerializeField] public AudioClip enemyShootSFX;
    [SerializeField] [Range(0, 1)] float shootVolume = 0.75f;



    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
        
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);

        }
    }

    private void Fire()
    {
        GameObject enemyBullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity) as GameObject;

        enemyBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyBulletSpeed);
        AudioSource.PlayClipAtPoint(enemyShootSFX, Camera.main.transform.position, shootVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        if (_showVFX)
        {
            GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(explosion, durationOfExplosion);
        }
        
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
    }
}
