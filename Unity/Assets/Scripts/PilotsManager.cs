using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class PilotsManager : MonoBehaviour
{

    private const float START_SHIFT = 8;
    private const float FINISH_SHIFT = 20;

    [SerializeField] private float ShiftTimer;
    
    [SerializeField] private AirplaneData[] airplanes;
    [SerializeField] private string[] pilotsName;
    [SerializeField] private Location[] Runways;
    [SerializeField] private Location[] Garages;

    [SerializeField, InfoBox("X:Time in hours(0.01 = 1h)\nY:Spawn rate in units per min(0.1 = 1 unit)")]
    private AnimationCurve rushHours;

    private List<Pilot> OnAir;
    private List<Pilot> OnRunway;
    private List<Pilot> OnGarage;

    public float CurrentCurveTime
    {
        get
        {
            float shiftTime = 0.01f * (FINISH_SHIFT-START_SHIFT);

            return (START_SHIFT * 0.01f)+((timer*shiftTime)/ShiftTimer);
        }
    }

    public TimeSpan CurrentRealTime
    {
        get
        {
            return TimeSpan.FromHours(CurrentCurveTime*100);
        }
    }
    
    /*public float SpawnRate
    {
        get
        {
            
        }
    }*/

    private float timer;

    void Awake()
    {
        OnAir = new List<Pilot>();
        OnRunway = new List<Pilot>();
        OnGarage = new List<Pilot>();
        timer = 0;

    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= ShiftTimer)
        {
            FinishShift();
        }
        Debug.Log(CurrentRealTime);
    }

    private void FinishShift()
    {
        Debug.Log("Finish");
    }

    private void GeneratePlaine()
    {
        List<string> numbersInUse = new List<string>();
        AirplaneData airplane = airplanes[UnityEngine.Random.Range(0, airplanes.Length)];

        foreach(Pilot pilot in OnAir)
            numbersInUse.Add(pilot.Number);
        foreach(Pilot pilot in OnRunway)
            numbersInUse.Add(pilot.Number);
        foreach(Pilot pilot in OnGarage)
            numbersInUse.Add(pilot.Number);
        
        OnAir.Add(new Pilot( airplane, numbersInUse, pilotsName));
    }
}
