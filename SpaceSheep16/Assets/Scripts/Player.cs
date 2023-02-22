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
    [SerializeField] int damage = 100;
    [SerializeField] private bool _wobble = false;
    private Tween _moveOutTween;

    [Header("Projectile")]
    [SerializeField] private bool _isSfxOn;
    [SerializeField] private bool _autoAttack = false;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] private Vector3 _bulletSpawnOffset;

    [Header("Player Death SFX")]
    [SerializeField] public AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathVolume = 0.75f;

    [Header("Player Shoot SFX")]
    [SerializeField] public AudioClip playerShootSFX;
    [SerializeField] [Range(0, 1)] float shootVolume = 0.75f;

    [Header("Color On Hit")]
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _colorChangeTime;
    [SerializeField] private Color _hitColor;
    [SerializeField] private bool _colorOnHit;
    
    [Header("Bounce On Hit")]
    [SerializeField] private bool _bounceOnHit;
    [SerializeField] private float _bounceDistance;
    [SerializeField] private float _bounceTime;
    [SerializeField] private Ease _bounceEase;
    private Tween _bounceTween;

    [Header("ScreenShake")]
    [SerializeField] private bool _isShakeOpen;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();

        if (_autoAttack)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        if(health <= 0)
            return;
        
        if(_bounceOnHit) BounceOnHit();
        if(_wobble) Wobble();
        if(_colorOnHit) ColorOnHit();
        if(_isShakeOpen) ScreenShake.Instance.PlayerGetHitEffect();
        
        health -= damage;
        if (health <= 0)
            Die();
    }

    private void BounceOnHit()
    {
        _bounceTween.Kill(false);
        Vector3 nextPos = transform.position + Vector3.down * _bounceDistance;
        _bounceTween = transform.DOMove(nextPos, _bounceTime).SetEase(_bounceEase);
    }
    
    private void Wobble()
    {
        transform.DOScale(new Vector3(1.5f, 0.5f, 1), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOScale(new Vector3(0.5f, 1.5f, 1), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(new Vector3(1f, 1f, 1), 0.1f).SetEase(Ease.Linear);
            });
        });
    }

    private void ColorOnHit()
    {
        _renderer.DOColor(_hitColor, _colorChangeTime * 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            _renderer.DOColor(Color.white, _colorChangeTime * 0.5f).SetEase(Ease.Linear);
        });
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
        if (Input.GetButtonDown("Fire1") && !_autoAttack)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if(Input.GetButtonUp("Fire1") && !_autoAttack)
        {
            StopCoroutine(firingCoroutine);
        }

    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + _bulletSpawnOffset, Quaternion.identity) as GameObject; // instantiate as Object to as GameObject 
                                                                                                                  //rotation can be given from here. Quaternion.identity means no rotation.
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            if(_isSfxOn) AudioSource.PlayClipAtPoint(playerShootSFX, Camera.main.transform.position, shootVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    [Header("Smooth Dumping")]
    [SerializeField] private bool _useSmoothEnd;
    private Single _moveX;
    private Single _moveY;
    private Vector2 _moveDirection;
    [SerializeField] float _dumpScaler = 1;
    [SerializeField] private float _dumpTime = 1;
    [SerializeField] private Ease _dumpEase = Ease.Linear;
    private void Move()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");

        if (_moveX != 0 || _moveY != 0)
            OnMoveInput();
        else if (_useSmoothEnd && _moveDirection.magnitude > 0)
            OnMoveInputExit();
    }

    private void OnMoveInput()
    {
        _bounceTween.Kill(false);
        _moveOutTween.Kill(false);
        
        var deltaX = _moveX * Time.deltaTime * moveSpeed;
        var deltaY = _moveY * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
        _moveDirection = new Vector2(deltaX, deltaY);
        _moveDirection.Normalize();
    }
    
    
    private void OnMoveInputExit()
    {
        //_bounceTween.Kill(false);
        _moveOutTween.Kill(false);
        
        _moveDirection *= _dumpScaler;

        Vector3 currentPos = transform.position;
        Vector3 nextPos = new Vector3(currentPos.x + _moveDirection.x, currentPos.y + _moveDirection.y);
        _moveOutTween = transform.DOMove(nextPos, _dumpTime).SetEase(_dumpEase);

        _moveDirection = Vector2.zero;
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

}
