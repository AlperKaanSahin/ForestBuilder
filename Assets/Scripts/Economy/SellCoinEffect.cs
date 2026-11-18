using System.Collections;
using UnityEngine;

public class SellCoinEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject coinVisualPrefab;
    [SerializeField] private Transform player;

    [Header("Animation")]
    [SerializeField] private float flyDuration = 0.3f;
    [SerializeField] private float arcHeight = 1f;

    public IEnumerator PlayCoinEffect(Vector3 startPosition)
    {
        GameObject coin = Instantiate(
            coinVisualPrefab,
            startPosition,
            Quaternion.identity
        );

        Vector3 targetPosition = player.position + Vector3.up;

        float elapsedTime = 0f;

        while (elapsedTime < flyDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsedTime / flyDuration
            );

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            Vector3 position = Vector3.Lerp(
                startPosition,
                targetPosition,
                smoothT
            );

            position.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

            coin.transform.position = position;

            coin.transform.Rotate(
                0f,
                360f * Time.deltaTime,
                0f
            );

            yield return null;
        }

        Destroy(coin);
    }
}