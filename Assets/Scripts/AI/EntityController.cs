using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIController))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EntityController : MonoBehaviour
{
    [HideInInspector]
    public CharacterScriptable myCharacter;
    [HideInInspector]
    public EntityMetadata myData;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public double suspicionMeter = 0;
    [HideInInspector]
    public bool amVector, amControlled;
    #region AIComponents
    AIController myAI;
    [HideInInspector]
    public NavMeshAgent agent;
    NavMeshPath path;
    Animator anim;
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
    [HideInInspector]
    public EntityActionStatus curActionStatus = EntityActionStatus.baseline;
    [HideInInspector]
    public Quaternion targetRot;
    int daysInfected, dayOfInfection;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        myAI = this.GetComponent<AIController>();
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        path = new NavMeshPath();
        curTask = null;
        //Update my outfit based on what my job is
    }

    // Update is called once per frame
    void Update()
    {
        if (amVector)
        {
            daysInfected = GameManager.instance.DataVariables.curDay - dayOfInfection;
        }
        if (!amControlled)
            RunControlManager();
        hunger -= Time.deltaTime * .13f;
        thirst -= Time.deltaTime * .21f;
        sleep -= Time.deltaTime * .1f;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            ReachedDestination();
        if(hunger <= 0 || thirst <= 0 || sleep <= 0)
        {
            suspicionMeter += Time.deltaTime * (.08f * daysInfected);
        }
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
                    StopTask();
                    if (thirst < 50)
                        FindHydration();
                    else if (sleep < 50)
                        FindSleep();
                    else if (hunger < 50)
                        FindFood();
                }
            }
        }
        else
        {
            StopTask();
            if (thirst < 50)
                FindHydration();
            else if (sleep < 50)
                FindSleep();
            else if (hunger < 50)
                FindFood();
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
        foreach (var item in GameManager.instance.DataVariables.taskObjects)
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
        agent.ResetPath();
        //Fire animation to stand back up
    }
    public void FindFood()
    {
        curActionStatus = EntityActionStatus.goingToCafeteria;
        agent.SetDestination(GameManager.instance.Locations.CafeteriaEntrance.position);
        targetRot = GameManager.instance.Locations.CafeteriaEntrance.rotation;
    }
    public void FindSleep()
    {
        curActionStatus = EntityActionStatus.goingToBed;
        agent.SetDestination(myBed.bedPoint.position);
        targetRot = myBed.bedPoint.transform.rotation;
    }
    public void FindHydration()
    {
        curActionStatus = EntityActionStatus.goingToWater;
        float dist = 1000;
        Transform targPoint = null;
        foreach (var item in GameManager.instance.Locations.WaterStandPoints)
        {
            if(Vector3.Distance(transform.position, item.position) < dist)
            {
                targPoint = item;
                dist = Vector3.Distance(transform.position, item.position);
            }
        }
        agent.SetDestination(targPoint.position);
        targetRot = targPoint.rotation;
    }
    public void ReachedDestination()
    {
        switch (curActionStatus)
        {
            case EntityActionStatus.goingToCafeteria:
                curActionStatus = EntityActionStatus.gettingFood;
                this.transform.rotation = targetRot;
                anim.SetTrigger("GetFood"); //runs CollectedFood()
                break;
            case EntityActionStatus.goingToBed:
                this.transform.rotation = targetRot;
                anim.SetTrigger("Sleep"); //runs FinishedSleep()
                break;
            case EntityActionStatus.goingToWater:
                this.transform.rotation = targetRot;
                anim.SetTrigger("Drink"); //runs FinishedHydration()
                break;
            case EntityActionStatus.gettingFood:
                this.transform.rotation = targetRot;
                curActionStatus = EntityActionStatus.eatingFood;
                anim.SetTrigger("EatFood"); //runs FinishedEat()
                break;
            case EntityActionStatus.goingToTask:
                break;
        }
    }
    #region AnimationTriggers
    public void FinishedHydration()
    {
        thirst = 100;
        curActionStatus = EntityActionStatus.baseline;
    }
    public void FinishedSleep()
    {
        sleep = 100;
        curActionStatus = EntityActionStatus.baseline;
    }
    public void FinishedEat()
    {
        hunger = 100;
        if (thirst <= 50)
            thirst = 75;
        curActionStatus = EntityActionStatus.baseline;
        GameManager.instance.FinishedEat(this);
    }
    public void CollectedFood() => GameManager.instance.RequestSeat(this);
    #endregion
    #region variables
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
    #endregion
}