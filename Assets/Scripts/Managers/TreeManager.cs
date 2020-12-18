using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

[AddComponentMenu("TimberMan/Managers/Tree Manager")]
public class TreeManager : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float startYPosition = -3;
    [SerializeField] float prefabYSize = 1.2f;

    [Header("Prefabs")]
    [SerializeField] Trunk trunkPrefab = default;
    [SerializeField] int numberFirstsNormalTrunks = 5;
    [SerializeField] List<Trunk> prefabs = new List<Trunk>();

    [Header("Pooling")]
    [SerializeField] int numberInitEveryPooling = 50;
    [SerializeField] int numberElementsInScene = 50;

    Dictionary<Trunk, Pooling<Trunk>> poolings = new Dictionary<Trunk, Pooling<Trunk>>();

    float yPosition;
    Queue<Trunk> elementsInScene = new Queue<Trunk>();
    Trunk lastTrunkInScene;

    void Awake()
    {
        yPosition = startYPosition;

        //init
        CreatePoolings();
        CreateFirstsTrunks();

        //start generate trunks
        GenerateTrunksInScene();
    }

    #region awake

    void CreatePoolings()
    {
        //be sure there is normal trunk in prefabs
        if (prefabs.Contains(trunkPrefab) == false)
            prefabs.Add(trunkPrefab);

        //foreach prefab
        foreach (Trunk prefab in prefabs)
        {
            //create pooling with few elements
            Pooling<Trunk> pool = new Pooling<Trunk>();
            pool.Init(prefab, numberInitEveryPooling);

            //add to dictionary
            poolings.Add(prefab, pool);
        }
    }

    void CreateFirstsTrunks()
    {
        //instantiate a few normal trunks
        for (int i = 0; i < numberFirstsNormalTrunks; i++)
        {
            GenerateTrunk(true);

            //increase y position
            yPosition += prefabYSize;
        }
    }

    void GenerateTrunksInScene()
    {
        //instantiate random prefabs
        for (int i = 0; i < numberElementsInScene; i++)
        {
            GenerateTrunk(false);

            //increase y position
            yPosition += prefabYSize;
        }
    }

    #endregion

    void GenerateTrunk(bool forceNormalTrunk)
    {
        //get prefab (normal or random)
        Trunk prefab = forceNormalTrunk ? trunkPrefab : prefabs[Random.Range(0, prefabs.Count)];

        //be sure is not impossible (if last one kill to right, next one can't kill to left, so put normal trunk between)
        if (lastTrunkInScene != null)
        {
            if ((lastTrunkInScene.KillToRight && prefab.KillToLeft) || (lastTrunkInScene.KillToLeft && prefab.KillToRight))
                prefab = trunkPrefab;
        }

        //instantiate prefab at position
        Trunk trunk = poolings[prefab].Instantiate(prefab, new Vector3(0, yPosition, 0), Quaternion.identity);

        //set last element in scene and add to queue
        lastTrunkInScene = trunk;
        elementsInScene.Enqueue(trunk);
    }

    #region public API

    public void PlayerChop()
    {
        //chop trunk - remove from queue and destroy it
        Trunk choppedTrunk = elementsInScene.Dequeue();
        Pooling.Destroy(choppedTrunk.gameObject);

        //generate new trunk
        GenerateTrunk(false);

        //foreach element in scene, move y down
        foreach (Trunk tr in elementsInScene)
        {
            tr.transform.position = new Vector3(tr.transform.position.x, tr.transform.position.y - prefabYSize, tr.transform.position.z);
        }
    }

    public bool PlayerDieOnChop(bool rightTap)
    {
        //check if next trunk kill player
        Trunk nextTrunk = elementsInScene.Peek();
        return (rightTap && nextTrunk.KillToRight) || (!rightTap && nextTrunk.KillToLeft);
    }

    #endregion
}
