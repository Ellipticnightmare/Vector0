using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIController))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EntityController : MonoBehaviour
{
    public EntityMetadata myData;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public double suspicionMeter = 0;
    [HideInInspector]
    public bool amVector, amControlled, selfcare;
    #region AIComponents
    AIController myAI;
    NavMeshAgent agent;
    NavMeshPath path;
    [HideInInspector]
    public Vector3 targetPosition;
    [HideInInspector]
    public TaskScriptable curTask;
    [HideInInspector]
    public JobScriptable myJob;
    List<Object> objects = new List<Object>();
    float hunger, sleep, thirst = 100;
    [HideInInspector]
    public BedPoint myBed;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        myAI = this.GetComponent<AIController>();
        agent = this.GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        curTask = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!amControlled && !selfcare)
            RunControlManager();
        hunger -= Time.deltaTime * .13f;
        thirst -= Time.deltaTime * .21f;
        sleep -= Time.deltaTime * .1f;
    }
    void OnMouseDown()
    {
        if (amVector)
        {
            VectorController.instance.myCurEntity = this;
            myAI.enabled = false;
        }
        else
            VectorController.instance.displayedEntity = this;
    }
    public void RunControlManager()
    {
        if (sufficientHunger() && sufficientSleep() && sufficientHydration())
        {
            if (curTask == null)
            {
                if (hunger >= 50 && sleep >= 50 && thirst >= 50)
                    AssignNewTask();
                else
                {
                    selfcare = true;
                    if (thirst < 50)
                        FindHydration();
                    else if (sleep < 50)
                        FindSleep();
                    else if (hunger < 50)
                        FindFood();
                }
            }
            else
            {
                StopTask();
            }
        }
    }
    public void AssignNewTask()
    {
        FilterTasks();
        FindPriorityTask();
    }
    public void FilterTasks()
    {
        objects.Clear();
        foreach (var item in GameManager.instance.taskObjects)
        {
            if (myJob.jobTasks.Contains(item.myTask))
                objects.Add(item);
        }
    }
    public void FindPriorityTask()
    {
        float min = 101;
        Object checkObj = objects[0];
        foreach (var item in objects)
        {
            if (item.progress < min)
            {
                checkObj = item;
                min = (float)item.progress;
            }
        }
        receivedEntityTask(checkObj.myTask, checkObj.transform.position);
    }
    public void UnselectChar() => myAI.enabled = true;
    public void receivedVectorTask(VectorTasks task)
    {
        StopTask();
    }
    public void receivedEntityTask(TaskScriptable task, Vector3 targPosition)
    {
        SetDestination(targPosition);
        curTask = task;
    }
    public void SetDestination(Vector3 position) => agent.SetDestination(position);
    public void StopTask()
    {
        curTask = null;
        //Fire animation to stand back up
    }
    public void FindFood()
    {

    }
    public void FinishedEat()
    {
        hunger = 100;
        selfcare = false;
        if (thirst <= 50)
            thirst = 75;
    }
    public void FindSleep()
    {
        agent.SetDestination(myBed.bedPoint.position);
    }
    public void FinishedSleep()
    {
        sleep = 100;
        selfcare = false;
    }
    public void FindHydration()
    {

    }
    public void FinishedHydration()
    {
        thirst = 100;
        selfcare = false;
    }
    public bool sufficientHunger()
    {
        return hunger >= 36;
    }
    public bool sufficientSleep()
    {
        return sleep >= 19;
    }
    public bool sufficientHydration()
    {
        return thirst >= 34;
    }
}
[System.Serializable]
public class EntityMetadata
{
    public Sprite myPortrait;
    public string myName;
}