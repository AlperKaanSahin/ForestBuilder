using UnityEngine;

public class TreeResource : MonoBehaviour
{
    private int currentHealth;
    private int woodReward;
    private bool isInitialized;
    private bool isChopped;


    public int WoodReward => woodReward;

    public event System.Action Chopped;

    private TreeVisual treeVisual;

    private void Awake()
    {
        treeVisual = GetComponent<TreeVisual>();
    }

    public void Initialize(int health, int woodAmount, float scale)
    {
        currentHealth = health;
        woodReward = woodAmount;

        transform.localScale = Vector3.one * scale;
        transform.rotation = Quaternion.Euler(
            0f,
            Random.Range(0f, 360f),
            0f
        );

        treeVisual?.SelectRandomVariant();

        isInitialized = true;

        Debug.Log(
            $"{name} initialized | Health: {currentHealth}, " +
            $"Wood: {woodReward}, Scale: {scale:F2}"
        );
    }

    public bool TakeDamage(int damage)
    {
        if (!isInitialized || isChopped)
            return false;

        currentHealth -= damage;

        Debug.Log($"{name} hit! Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            ChopDown();
            return true;
        }

        return false;
    }

    private void ChopDown()
    {
        if (isChopped)
            return;

        isChopped = true;

        Debug.Log($"{name} chopped down! Wood: {woodReward}");

        Chopped?.Invoke();

        Destroy(gameObject);
    }
}