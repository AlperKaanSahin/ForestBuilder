using System.Collections.Generic;
using UnityEngine;

public class ChopDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 4f;
    [SerializeField] private float chopInterval = 0.5f;
    [SerializeField] private int damagePerHit = 1;

    private readonly List<TreeResource> treesInRange = new List<TreeResource>();

    private TreeResource currentTree;
    private float chopTimer;

    private void Update()
    {
        DetectTrees();

        if (currentTree == null)
        {
            SelectNextTree();
            return;
        }

        chopTimer += Time.deltaTime;

        if (chopTimer >= chopInterval)
        {
            bool choppedDown = currentTree.TakeDamage(damagePerHit);

            if (choppedDown)
            {
                currentTree = null;
            }

            chopTimer = 0f;
        }
    }

    private void DetectTrees()
    {
        treesInRange.Clear();

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            detectionRadius
        );

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Tree"))
                continue;

            TreeResource tree = collider.GetComponent<TreeResource>();

            if (tree != null && !treesInRange.Contains(tree))
            {
                treesInRange.Add(tree);
            }
        }

        if (currentTree != null && !treesInRange.Contains(currentTree))
        {
            currentTree = null;
            chopTimer = 0f;
        }
    }

    private void SelectNextTree()
    {
        if (treesInRange.Count == 0)
            return;

        currentTree = treesInRange[0];
        chopTimer = 0f;

        Debug.Log($"Started chopping {currentTree.name}");
    }
}