using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;
    [SerializeField] int damage = 100;

    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] public GameObject enemyBulletPrefab;
    [SerializeField] float enemyBulletSpeed = 2f;

    [Header("Juice")]
    [SerializeField] private bool _scaleUpOnDestroy;
    [SerializeField] private bool _showVFX;
    
    [Header("Color On Hit")]
    [SerializeField] private bool _changeColorOnHit;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _colorChangeTime;
    [SerializeField] private Color _hitColor;
    [SerializeField] private Color _defaultColor;
    
    [Header("Bounce On Hit")]
    [SerializeField] private bool _bounceOnHit;
    [SerializeField] private float _bounceDistance;
    [SerializeField] private float _bounceTime;
    [SerializeField] private Ease _bounceEase;
    private Tween _bounceTween;
    
    [Header("Death VFX")]
    [SerializeField] public GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] private bool _shakeCamera;
    [SerializeField] private bool _canvasShake;

    [Header("Enemy Death SFX")]
    [SerializeField] public AudioClip deathSFX;
    [SerializeField] [Range(0,1)] float deathVolume = 0.75f;

    [Header("Enemy Shoot SFX")]
    [SerializeField] public bool _isSfxOn;
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
        if(_isSfxOn) AudioSource.PlayClipAtPoint(enemyShootSFX, Camera.main.transform.position, shootVolume);
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
        if (_changeColorOnHit) ColorOnHit();
        
        health -= damage;
        if (health <= 0)
            Die();
    }
    
    private void BounceOnHit()
    {
        _bounceTween.Kill(false);
        Vector3 nextPos = transform.position + Vector3.up * _bounceDistance;
        _bounceTween = transform.DOMove(nextPos, _bounceTime).SetEase(_bounceEase);
    }
    
    private void ColorOnHit()
    {
        _renderer.DOColor(_hitColor, _colorChangeTime * 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            _renderer.DOColor(_defaultColor, _colorChangeTime * 0.5f).SetEase(Ease.Linear);
        });
    }
    
    private void Die()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        health = -1000;
        
        FindObjectOfType<GameSession>().AddToScore(scoreValue);

        if (_scaleUpOnDestroy)
            ScaleUpAndDie();
        else
        {
            if (_showVFX)
            {
                GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
                Destroy(explosion, durationOfExplosion);
                
                if(_shakeCamera) ScreenShake.Instance.EnemyHitShakeEffect();
                if(_canvasShake) CanvasShake.Instance.CanvasGetShake();
            }
            if(_isSfxOn) AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
            Destroy(gameObject);
        }
        
    }

    private void ScaleUpAndDie()
    {
        Vector3 bigScale = new Vector3(2, 2, 2);
        transform.DOScale(bigScale, 0.5f).From().SetEase(Ease.OutCirc).OnKill(() =>
        {
            if (_showVFX)
            {
                GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
                Destroy(explosion, durationOfExplosion);
                
                if(_shakeCamera) ScreenShake.Instance.EnemyHitShakeEffect();
            }
            if(_isSfxOn) AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
            Destroy(gameObject);
        });
    }
}
