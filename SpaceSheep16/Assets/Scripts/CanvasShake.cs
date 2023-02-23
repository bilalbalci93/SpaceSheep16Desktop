using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanvasShake : MonoBehaviour
{
    #region Singleton

    public static CanvasShake Instance { get; private set; }

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

    [SerializeField] private float shakeDuration = 0.4f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    private Vector2 originalAnchoredPosition;

    void Start()
    {
        originalAnchoredPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void CanvasGetShake()
    {
        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        float elapsed = 1f;

        while (elapsed < shakeDuration)
        {
            float x = originalAnchoredPosition.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = originalAnchoredPosition.y + Random.Range(-1f, 1f) * shakeMagnitude;

            GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            elapsed += Time.deltaTime;

            yield return null;
        }

        GetComponent<RectTransform>().anchoredPosition = originalAnchoredPosition;
    }
}
