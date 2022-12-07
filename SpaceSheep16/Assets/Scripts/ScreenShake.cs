using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    #region Singleton

    public static ScreenShake Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            Instance = this;
        }
    }

    #endregion

    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _slowDownTime = 0.5f;
    [SerializeField] private float _slowTimeScale = 0.2f;

    private Vector3 _defaultCameraPos;

    private void Start()
    {
        _defaultCameraPos = _camera.transform.position;
    }

    public void PlayerGetHitEffect()
    {
        StartCoroutine(SlowDown());
        _camera.transform.DOShakePosition(_slowDownTime, 1f);
    }

    private IEnumerator SlowDown()
    {
        Time.timeScale = _slowTimeScale;
        
        yield return new WaitForSeconds(_slowDownTime);
        Time.timeScale = 1;
    }

    public void EnemyHitShakeEffect()
    {
        _camera.transform.DOShakePosition(_slowDownTime * 0.2f, 0.2f);
    }
}
