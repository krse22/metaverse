using Prototyping.Games;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class RunnerManagerBase : MonoBehaviour
{

    [SerializeField] protected UnityEvent onGameEndEvent;

    [SerializeField] protected Transform player;
    [SerializeField] protected Transform startPosition;

    [SerializeField] protected InfinityRunnerSpawnSystem[] spawnSystems;

    [SerializeField] protected int laneCount;
    [SerializeField] protected float sideDashDistance;
    [SerializeField] protected float movementSpeed;
    [SerializeField] private float offsetDeleteFix = 0;

    protected bool isPlaying = false;
    protected PlayerRunnerController controller;

    public float MovementSpeed =>  movementSpeed;
    public bool IsPlaying => isPlaying;
    public float PlayerZ => startPosition.position.z; 
    public float OffsetDeleteFix => offsetDeleteFix;

    [SerializeField] protected Transform objectsParent;

    public abstract void OnGameEnd();

    public void RegisterObject(GameObject go)
    {
        go.transform.SetParent(objectsParent);
    }

    protected void InitSystems()
    {
        spawnSystems.ToList().ForEach((s) => s.Initialize(this));
    }

    // Called on Play from a button
    // If this is a 5 lane manager, 3 lane manager UI button will call this
    // if this is a 3 lane manager, 5 lane manager UI button will call this
    public void ObjectCleanup()
    {
        foreach (Transform child in objectsParent)
        {
            Destroy(child.gameObject);
        }
    }


}
