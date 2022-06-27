using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(AIController))]
[RequireComponent(typeof(NavMeshAgent))]
public class EntityController : MonoBehaviour
{
    public EntityMetadata myData;
    [HideInInspector]
    public int health;
    AIController myAI;
    NavMeshAgent agent;
    NavMeshPath path;
    bool amVector;
    // Start is called before the first frame update
    void Start()
    {
        myAI = this.GetComponent<AIController>();
        agent = this.GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseDown()
    {
        if (amVector)
        {
            VectorController.instance.myCurEntity = this;
            myAI.enabled = false;
            if (VectorController.instance.curTask != EntityTasks.empty)
            {
                receivedTask(VectorController.instance.curTask);
            }
        }
        else
            VectorController.instance.displayedEntity = this;
    }
    public void UnselectChar() => myAI.enabled = true;
    public void receivedTask(EntityTasks task)
    {

    }
}
[System.Serializable]
public class EntityMetadata
{
    public Image myPortrait;
    public Text myName;
}