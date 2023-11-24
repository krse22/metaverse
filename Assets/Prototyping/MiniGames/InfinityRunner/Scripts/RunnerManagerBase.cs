using Prototyping.Games;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class RunnerManagerBase : MonoBehaviour
{
    [SerializeField] protected Transform player;
    [SerializeField] protected Transform startPosition;

    [SerializeField] protected InfinityRunnerSpawnSystem[] spawnSystems;

    [SerializeField] protected int laneCount;
    [SerializeField] protected float sideDashDistance;
    [SerializeField] protected float movementSpeed;
    [SerializeField] private float offsetDeleteFix = 0;

    [SerializeField] protected InfinityRunnerManagerCurrent current;

    protected bool isPlaying = false;
    protected PlayerRunnerController controller;

    public float MovementSpeed =>  movementSpeed;
    public bool IsPlaying => isPlaying;
    public float PlayerZ => startPosition.position.z; 
    public float OffsetDeleteFix => offsetDeleteFix;

    public int LaneCount => laneCount;

    [SerializeField] protected Transform objectsParent;

    public abstract void OnGameEnd();
    public abstract void OnGameStart();

    public void RegisterObject(GameObject go)
    {
        go.transform.SetParent(objectsParent);
    }

    protected void InitSystems()
    {
        spawnSystems.ToList().ForEach((s) => s.Initialize(this));
    }

    public void ObjectCleanup()
    {
        foreach (Transform child in objectsParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void Pause()
    {
        isPlaying = false;
    }

    public void Unpause()
    {
        isPlaying = true;
    }

}
