using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VectorController))]
public class GameManager : MonoBehaviour
{
    #region Sounds
    [Header("Sounds")]
    public AudioClip powerDownSound;
    public AudioClip powerDownDeepSound;
    public AudioSource[] SFXSounds;
    #endregion
    #region Lights
    [Header("Lights")]
    public bool hasPrimaryPower;
    public bool hasEmergencyPower;
    public Light[] primaryLights;
    public Light[] emergencyLights;
    public Object[] emergencyGenerators;
    public Object[] primaryGenerators;
    #endregion
    #region Map Locations and Data
    [Header("Locations")]
    public Collider Cafeteria;
    public Collider[] GeneratorRooms;
    public Collider[] BedRooms;
    public BedPoint[] BedPoints;
    public Transform[] WaterPoints;
    #endregion
    #region Gameplay Data
    [Header("Data Variables")]
    public int DNApoints = 0;
    public Object[] taskObjects;
    public static GameManager instance;
    #endregion
    private void Start()
    {
        instance = this;
        taskObjects = FindObjectsOfType<Object>();
        EntityController[] entities = FindObjectsOfType<EntityController>();
        int x = Random.Range(0, entities.Length);
        entities[x].amVector = true;
        entities[x].suspicionMeter += 4.2f;
    }
    private void Update()
    {
        #region Lights
        hasPrimaryPower = false;
        hasEmergencyPower = false;
        foreach (var item in primaryGenerators)
        {
            if (item.progress > 0)
                hasPrimaryPower = true;
        }
        foreach (var item in emergencyGenerators)
        {
            if (item.progress > 0)
                hasEmergencyPower = true;
        }
        if (!hasPrimaryPower)
        {
            foreach (var item in primaryLights)
            {
                if (item.enabled)
                {
                    PlaySoundOnce(powerDownSound);
                    item.enabled = false;
                }
            }
        }
        else
        {
            foreach (var item in primaryLights)
                item.enabled = true;
        }
        if(!hasPrimaryPower && hasEmergencyPower)
        {
            foreach (var item in emergencyLights)
                item.enabled = true;
        }
        if (!hasEmergencyPower)
        {
            foreach (var item in emergencyLights)
            {
                if (item.enabled)
                {
                    PlaySoundOnce(powerDownDeepSound);
                    item.enabled = false;
                }
            }
        }
        #endregion
    }
    public void PlaySoundOnce(AudioClip clip)
    {
        foreach (var item in SFXSounds)
        {
            if (item.clip == null)
            {
                item.clip = clip;
                item.Play();
                break;
            }
        }
    }
}