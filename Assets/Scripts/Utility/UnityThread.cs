using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;


public class UnityThread : MonoBehaviour
{
    //our (singleton) instance
    public static UnityThread Instance { get; private set; } = null;

    private static Thread unityThread;


    ////////////////////////////////////////////////UPDATE IMPL////////////////////////////////////////////////////////
    //Holds actions received from another Thread. Will be coped to actionCopiedQueueUpdateFunc then executed from there
    private static List<System.Action> actionQueuesUpdateFunc = new List<Action>();

    //holds Actions copied from actionQueuesUpdateFunc to be executed
    List<System.Action> actionCopiedQueueUpdateFunc = new List<System.Action>();

    // Used to know if whe have new Action function to execute. This prevents the use of the lock keyword every frame
    private volatile static bool noActionQueueToExecuteUpdateFunc = true;


    //Used to initialize UnityThread. Call once before any function here
    public static void Init(bool visible = false)
    {
        if (Instance != null)
        {
            return;
        }

        if (UnityEngine.Application.isPlaying)
        {
            // add an invisible game object to the scene
            GameObject obj = new GameObject("MainThreadExecuter");
            if (!visible)
            {
                obj.hideFlags = HideFlags.HideAndDontSave;
            }

            DontDestroyOnLoad(obj);
            Instance = obj.AddComponent<UnityThread>();


        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        unityThread = Thread.CurrentThread;
    }

    public void EnqueueTask(Action task)
    {
        ExecuteInUnityThread(task);
    }

    public static void ExecuteInUnityThread(System.Action action)
    {
        if (action == null)
            throw new ArgumentNullException("action");

        if (Thread.CurrentThread == unityThread)
        {
            action();
            return;
        }

        lock (actionQueuesUpdateFunc)
        {
            actionQueuesUpdateFunc.Add(action);
            noActionQueueToExecuteUpdateFunc = false;
        }
    }

    public void Update()
    {
        if (noActionQueueToExecuteUpdateFunc)
        {
            return;
        }

        //Clear the old actions from the actionCopiedQueueUpdateFunc queue
        actionCopiedQueueUpdateFunc.Clear();
        lock (actionQueuesUpdateFunc)
        {
            //Copy actionQueuesUpdateFunc to the actionCopiedQueueUpdateFunc variable
            actionCopiedQueueUpdateFunc.AddRange(actionQueuesUpdateFunc);
            //Now clear the actionQueuesUpdateFunc since we've done copying it
            actionQueuesUpdateFunc.Clear();
            noActionQueueToExecuteUpdateFunc = true;
        }

        // Loop and execute the functions from the actionCopiedQueueUpdateFunc
        for (int i = 0; i < actionCopiedQueueUpdateFunc.Count; i++)
        {
            actionCopiedQueueUpdateFunc[i].Invoke();
        }
    }

    public void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}