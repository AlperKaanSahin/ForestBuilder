using System.Collections;
using UnityEngine;

public class UpgradeCoinEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject coinVisualPrefab;
    [SerializeField] private Transform player;

    [Header("Animation")]
    [SerializeField] private float flyDuration = 0.25f;
    [SerializeField] private float arcHeight = 1f;

    public IEnumerator PlayCoinEffect()
    {
        GameObject coin = Instantiate(
            coinVisualPrefab,
            player.position + Vector3.up,
            Quaternion.identity
        );

        Vector3 startPosition = coin.transform.position;
        Vector3 targetPosition = transform.position + Vector3.up * 0.5f;

        float elapsedTime = 0f;

        while (elapsedTime < flyDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / flyDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            Vector3 position = Vector3.Lerp(
                startPosition,
                targetPosition,
                smoothT
            );

            // Coin'in düz çizgi yerine küçük bir yay çizmesini sağlar.
            position.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

            coin.transform.position = position;

            // Uçarken biraz dönsün.
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