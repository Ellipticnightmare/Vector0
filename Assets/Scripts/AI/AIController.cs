using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [HideInInspector]
    public JobScriptable myJob;
    EntityController myController;
    [HideInInspector]
    public List<AIStatus> myCurrentStatuses = new List<AIStatus>();
    // Start is called before the first frame update
    void Start()
    {
        myController = this.GetComponent<EntityController>();
        myCurrentStatuses.Add(AIStatus.Default);
    }

    // Update is called once per frame
    void Update()
    {
        /*check every frame to see other entities.  If I can, and they are being suspicious, increase
        their suspicion meter*/
        /*check every frame to see other entities.  If I can, and their suspicion meter is high enough,
        start an Alarm*/
    }
}