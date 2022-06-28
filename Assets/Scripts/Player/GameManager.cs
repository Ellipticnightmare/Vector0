using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VectorController))]
public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int tMod = 1;
    public static GameManager instance;
    public SoundsManager Sounds;
    public LightsManager Lights;
    public LocationsManager Locations;
    public DataVariablesManager DataVariables;
    public AIVariablesManager AIVariables;
    public UIComponentsManager UIComponents;
    private void Start()
    {
        instance = this;
        DataVariables.taskObjects = FindObjectsOfType<Object>();
        StartCoroutine(SetupGame());
    }
    IEnumerator SetupGame()
    {
        //Spawn Characters at spawn points
        //Assign each Character a job
        yield return new WaitForEndOfFrame();
        EntityController[] entities = FindObjectsOfType<EntityController>();
        int x = Random.Range(0, entities.Length);
        entities[x].amVector = true;
        entities[x].suspicionMeter += 4.2f;
        yield return new WaitForEndOfFrame();
        UpdateInGame();
    }
    private void UpdateInGame()
    {
        #region Lights
        Lights.hasPrimaryPower = false;
        Lights.hasEmergencyPower = false;
        foreach (var item in Lights.primaryGenerators)
        {
            if (item.progress > 0)
                Lights.hasPrimaryPower = true;
        }
        foreach (var item in Lights.emergencyGenerators)
        {
            if (item.progress > 0)
                Lights.hasEmergencyPower = true;
        }
        if (!Lights.hasPrimaryPower)
        {
            foreach (var item in Lights.primaryLights)
            {
                if (item.enabled)
                {
                    PlaySoundOnce(Sounds.powerDownSound);
                    item.enabled = false;
                }
            }
        }
        else
        {
            foreach (var item in Lights.primaryLights)
                item.enabled = true;
        }
        if (!Lights.hasPrimaryPower && Lights.hasEmergencyPower)
        {
            foreach (var item in Lights.emergencyLights)
                item.enabled = true;
        }
        if (!Lights.hasEmergencyPower)
        {
            foreach (var item in Lights.emergencyLights)
            {
                if (item.enabled)
                {
                    PlaySoundOnce(Sounds.powerDownDeepSound);
                    item.enabled = false;
                }
            }
        }
        #endregion
        #region TimeManagement
        if (DataVariables.curClockTime <= DataVariables.clockTimeGoal)
            DataVariables.curClockTime += Time.deltaTime * tMod;
        else
            AdvanceTime();
        #endregion
        #region Keyboard Controls
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
        #endregion
        UpdateInGame();
    }
    public void PlaySoundOnce(AudioClip clip)
    {
        foreach (var item in Sounds.SFXSounds)
        {
            if (item.clip == null)
            {
                item.clip = clip;
                item.Play();
                break;
            }
        }
    }
    public void TogglePause()
    {
        DataVariables.isPaused = !DataVariables.isPaused;
        tMod = DataVariables.isPaused ? 0 : 1;
        UIComponents.pauseUI.SetActive(DataVariables.isPaused);
    }
    public void RequestSeat(EntityController control)
    {
        foreach (var item in Locations.CafeteriaSeatPoints)
        {
            if (!item.hasAssigned)
            {
                item.hasAssigned = true;
                control.agent.SetDestination(item.standPoint.position);
                item.entity = control;
                break;
            }
        }
    }
    public void FinishedEat(EntityController control)
    {
        foreach (var item in Locations.CafeteriaSeatPoints)
        {
            if (item.hasAssigned)
            {
                if (item.entity = control)
                {
                    item.hasAssigned = false;
                    item.entity = null;
                    break;
                }
            }
        }
    }
    public void AdvanceTime()
    {
        DataVariables.curClockTime = 0;
        switch (DataVariables.curTime)
        {
            case TimeOfDay.dawn:
                DataVariables.curTime = TimeOfDay.morning;
                DataVariables.clockTimeGoal = DataVariables.morningTime;
                break;
            case TimeOfDay.morning:
                DataVariables.curTime = TimeOfDay.noon;
                DataVariables.clockTimeGoal = DataVariables.noonTime;
                break;
            case TimeOfDay.noon:
                DataVariables.curTime = TimeOfDay.afternoon;
                DataVariables.clockTimeGoal = DataVariables.afternoonTime;
                break;
            case TimeOfDay.afternoon:
                DataVariables.curTime = TimeOfDay.evening;
                DataVariables.clockTimeGoal = DataVariables.eveningTime;
                break;
            case TimeOfDay.evening:
                DataVariables.curTime = TimeOfDay.night;
                DataVariables.clockTimeGoal = DataVariables.nightTime;
                break;
            case TimeOfDay.night:
                DataVariables.curDay++;
                DataVariables.curTime = TimeOfDay.dawn;
                DataVariables.clockTimeGoal = DataVariables.dawnTime;
                break;
        }
    }
}
#region Externals
#region Classes
[System.Serializable]
public class EntityMetadata
{
    public Sprite myPortrait;
    public string myName;
}
[System.Serializable]
public class OutfitManager
{
    public JobScriptable thisJob;
    public Material thisOutfit;
}
[System.Serializable]
public class LightsManager
{
    [HideInInspector]
    public bool hasPrimaryPower, hasEmergencyPower;
    public Light[] primaryLights;
    public Light[] emergencyLights;
    public Object[] emergencyGenerators;
    public Object[] primaryGenerators;
}
[System.Serializable]
public class SoundsManager
{
    public AudioClip powerDownSound;
    public AudioClip powerDownDeepSound;
    public AudioSource[] SFXSounds;
}
[System.Serializable]
public class LocationsManager
{
    public Transform CafeteriaEntrance;
    public Collider[] BedRooms;
    public BedPoint[] BedPoints;
    public CafeteriaSeatPoint[] CafeteriaSeatPoints;
    public Transform[] WaterStandPoints, GeneratorStandPoints, PopulateSpawnPoints;
}
[System.Serializable]
public class DataVariablesManager
{
    public int curDay;
    [HideInInspector]
    public int DNApoints = 0;
    public Object[] taskObjects;
    public TimeOfDay curTime = TimeOfDay.night;
    public float clockTimeGoal, curClockTime;
    public Image todIndicator, todTimerIndicator;
    public int dawnTime, morningTime, noonTime, afternoonTime, eveningTime, nightTime;
    [HideInInspector]
    public int remainingHumans;
    [HideInInspector]
    public bool isPaused;
}
[System.Serializable]
public class AIVariablesManager
{
    public BaseType thisBase = BaseType.arctic;
    public OutfitManager[] outfitSystem;
    public JobDatabase jobs;
    public CharacterDatabase characters;
}
[System.Serializable]
public class UIComponentsManager
{
    public GameObject pauseUI;
}
#endregion
#region Enums
public enum AIStatus
{
    Default,
    Sleeping,
    Unconscious,
    Dead,
    Suspicious,
    Alarmed
};
public enum EntityActionStatus
{
    baseline,
    goingToCafeteria,
    gettingFood,
    eatingFood,
    goingToBed,
    goingToWater,
    goingToTask
};
public enum TimeOfDay
{
    dawn,
    morning,
    noon,
    afternoon,
    evening,
    night
};
public enum BaseType
{
    arctic,
    jungle,
    desert,
    urban
};
#endregion
#endregion