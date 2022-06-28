using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VectorController : MonoBehaviour
{
    public EntityController myCurEntity, displayedEntity;
    public static VectorController instance;
    public Object dispObject;
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
    #region Object
    public Text objName, progress;
    public Image objPortrait;
    #endregion
    #endregion
    #endregion
    [HideInInspector]
    public VectorTasks curTask = VectorTasks.empty;
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
            entityPortrait.sprite = myCurEntity.myData.myPortrait;
            entityName.text = myCurEntity.myData.myName;
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
        else if(dispObject != null)
        {
            objPortrait.sprite = dispObject.thisData.myPortrait;
            objName.text = dispObject.thisData.myName;
            progress.text = dispObject.progress.ToString();
        }
    }
    public void selectedTask(VectorTasks task)
    {
        curTask = task;
    }
}
public enum VectorTasks
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