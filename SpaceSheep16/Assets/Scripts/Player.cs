using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f; // might be different for vertical and horizontal. Top of the screen might be banned for player movement
    [SerializeField] int health = 200;

    [Header("Projectile")]
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    [Header("Player Death SFX")]
    [SerializeField] public AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathVolume = 0.75f;

    [Header("Player Shoot SFX")]
    [SerializeField] public AudioClip playerShootSFX;
    [SerializeField] [Range(0, 1)] float shootVolume = 0.75f;



    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; } // if there is no damage dealer then dont do further and return. It is for null error.
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit(); // this is for destroying the projectiles

        if (health <= 0)
        {
            
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
    }

    public int GetHealth()
    {
        return health;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }

    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject; // instantiate as Object to as GameObject 
                                                                                                                  //rotation can be given from here. Quaternion.identity means no rotation.
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(playerShootSFX, Camera.main.transform.position, shootVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    [Header("Smooth Dumping")]
    [SerializeField] private bool _useSmoothEnd;
    private Single _moveX;
    private Single _moveY;
    private Single _prevMoveX;
    private Single _prevMoveY;
    [SerializeField] float _dumpScaler = 1;
    [SerializeField] private float _dumpTime = 1;
    [SerializeField] private Ease _dumpEase = Ease.Linear;
    private void Move()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");

        if (_moveX != 0 || _moveY != 0)
            OnMoveInput();
        else if (_useSmoothEnd && (_prevMoveX != 0 || _prevMoveY != 0))
            OnMoveInputExit();
    }

    private void OnMoveInput()
    {
        DOTween.KillAll(false);
        
        var deltaX = _moveX * Time.deltaTime * moveSpeed;
        var deltaY = _moveY * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
        
        _prevMoveX = _moveX;
        _prevMoveY = _moveY;
    }
    
    
    private void OnMoveInputExit()
    {
        DOTween.KillAll(false);
        
        var deltaX = _prevMoveX * _dumpScaler;
        var deltaY = _prevMoveY * _dumpScaler;

        Vector3 currentPos = transform.position;
        Vector3 nextPos = new Vector3(currentPos.x + deltaX, currentPos.y + deltaY);
        transform.DOMove(nextPos, _dumpTime).SetEase(_dumpEase);

        _prevMoveX = 0;
        _prevMoveY = 0;
    }
    

    public SpriteRenderer renderer;

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

}
