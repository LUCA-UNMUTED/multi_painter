using Unity.Netcode;
using UnityEngine;

public class GrabbableCreator : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;

    [SerializeField]
    private Vector2 placementArea = new Vector2(-10.0f, 10.0f);

    [SerializeField]
    private Vector2 prefabSizes = new Vector2(0.01f, 2.0f);

    [SerializeField]
    private int maxObjectsToSpawn = 10;

    [SerializeField] private bool testSpawn = false;

    [SerializeField, Tooltip("empty prefab to spawn the cubes as childs")] private GameObject grabbableContainer;

    private void Start()
    {
        //if (IsServer || IsHost)
        //{
            Generate();
        //}
    }

    private void Update()
    {
        if (testSpawn)
        {
            testSpawn = false;
            Generate();
        }
    }

    private void Generate()
    {
        if (IsServer || IsHost)
        {
            for (int i = 0; i < maxObjectsToSpawn; i++)
            {
                GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Length)], Vector3.zero, Quaternion.identity);
                go.transform.position = new Vector3(Random.Range(placementArea.x, placementArea.y), 5,
                    Random.Range(placementArea.x, placementArea.y));
                float randomSize = Random.Range(prefabSizes.x, prefabSizes.y);
                go.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
                go.GetComponent<NetworkObject>().Spawn();
                go.GetComponent<NetworkObject>().TrySetParent(grabbableContainer.transform);
                go.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
