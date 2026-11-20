using UnityEngine;

public class RandomVisualSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] visualVariants;

    public void SelectRandomVariant()
    {
        if (visualVariants == null || visualVariants.Length == 0)
            return;

        foreach (GameObject variant in visualVariants)
        {
            if (variant != null)
                variant.SetActive(false);
        }

        int randomIndex = Random.Range(0, visualVariants.Length);
        Debug.Log(
    $"{name} selected visual: " +
    $"{visualVariants[randomIndex].name}"
);

        if (visualVariants[randomIndex] != null)
            visualVariants[randomIndex].SetActive(true);
    }
}