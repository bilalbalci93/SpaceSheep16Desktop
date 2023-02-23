using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float _slowDownTime = 0.3f;
    [SerializeField] private float _slowTimeScale = 0.2f;

    [Header("Canvas Shake")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _canvasTransform;
    [SerializeField] private float _shakeStrength;

    private Vector3 _defaultCameraPos;

    private Vector2 _defaultCanvasPos;

    private void Start()
    {
        _defaultCameraPos = _camera.transform.position;
        _defaultCanvasPos = _canvas.GetComponent<RectTransform>().anchoredPosition;
        //_canvasTransform = _canvas.GetComponent<RectTransform>().anchoredPosition;
    }

    public void PlayerGetHitEffect()
    {
        StartCoroutine(SlowDown());
        _camera.transform.DOShakePosition(_slowDownTime, 0.8f);
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

    public void UIGetsShakeEffect() 
    {
        //_canvas.GetComponent<RectTransform>().anchoredPosition.DOShakePosition(_slowDownTime * 0.2f, 0.2f);

        _canvas.GetComponent<RectTransform>().DOShakeAnchorPos(_slowDownTime * 0.2f, _shakeStrength);
    }


}
