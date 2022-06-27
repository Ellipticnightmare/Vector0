using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VectorController : MonoBehaviour
{
    public EntityController myCurEntity, displayedEntity;
    public static VectorController instance;
    #region UI
    public GameObject defaultDisplayUI, AIDisplayUI, EntityControlUI;
    #region EntityControl
    public Image entityPortrait;
    public Image[] healthPoints;
    public Text entityName;
    #endregion
    #region AIDisplay
    #endregion
    #region defaultDisplay
    #endregion
    #endregion
    [HideInInspector]
    public EntityTasks curTask = EntityTasks.empty;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(myCurEntity != null)
        {
            entityPortrait = myCurEntity.myData.myPortrait;
            entityName = myCurEntity.myData.myName;
            foreach (var item in healthPoints)
            {
                item.color = Color.red;
            }
            for (int i = 0; i < myCurEntity.health; i++)
            {
                healthPoints[i].color = Color.black;
            }
        }
        else if(displayedEntity != null)
        {

        }
    }
    public void selectedTask(EntityTasks task)
    {
        curTask = task;
    }
}
public enum EntityTasks
{
    empty,
    attack,
    infect,
    sabotage,
    feed,
    infectTrap,
    genReshuffle,
    mutate,
    assimilate,
    overlord
};