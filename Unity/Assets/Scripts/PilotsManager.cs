using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Random = UnityEngine.Random;
using TMPro;

public class PilotsManager : MonoBehaviour
{

    private const float START_SHIFT = 20;
    private const float FINISH_SHIFT = 5;

    [InfoBox("Tmer in seconds")]
    [SerializeField] private float ShiftTimer;
    
    [SerializeField] private AirplaneData[] airplanes;
    [SerializeField] private string[] pilotsName;
    [SerializeField] private Location[] Runways;
    [SerializeField] private Location[] Garages;

    [SerializeField, InfoBox("X:Time in hours(0.01 = 1h)\nY:Spawn rate in units per 20 min(0.1 = 1 unit)")]
    private AnimationCurve rushHours;

    [SerializeField] private List<Pilot> OnAir;
    [SerializeField] private List<Pilot> OnRunway;
    [SerializeField] private List<Pilot> OnGarage;
    [SerializeField] private TextMeshProUGUI clock;

    public float ShiftDuration
    {
        get
        {
            if(START_SHIFT < FINISH_SHIFT)
                return FINISH_SHIFT - START_SHIFT;
            else
            {
                return (24 - START_SHIFT) + FINISH_SHIFT;
            }
        }
    }

    public float CurrentCurveTime
    {
        get
        {
            return (CurrentRealTime.Hours * 0.01f);
        }
    }

    public TimeSpan CurrentRealTime
    {
        get
        {
            float currentTime = (START_SHIFT )+((timer*ShiftDuration)/ShiftTimer);
            
            return TimeSpan.FromHours(currentTime);
        }
    }
    
    public float SpawnRate
    {
        get
        {
            return rushHours.Evaluate(CurrentCurveTime) * 10;
        }
    }

    private float timer;

    void Awake()
    {
        OnAir = new List<Pilot>();
        OnRunway = new List<Pilot>();
        OnGarage = new List<Pilot>();
        timer = 0;

        int pilotsInAirport = Random.Range(0, Garages.Length);


        for(int i = 0; i< pilotsInAirport; i++)
        {
            GeneratePlane(OnGarage);

            int garage = 0;
            do
            {
                garage = Random.Range(0, Garages.Length);
            }while(Garages[garage].IsOccupied);

            Garages[garage].TryToMovePilotIn(OnGarage[OnGarage.Count-1]);
        }

        StartCoroutine(Spawn());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(CurrentRealTime.Hours == FINISH_SHIFT)
        {
            FinishShift();
        }
        string s = string.Format("{0,2:D2}:{1,2:D2}", CurrentRealTime.Hours , CurrentRealTime.Minutes);
        if(clock != null)
            clock.text = s;
    }

    private void FinishShift()
    {
        Debug.Log("Finish");
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(((ShiftTimer/ShiftDuration)/3)/SpawnRate);
            GeneratePlane(OnAir);
            print("Spawn");
        }
    }

    private void GeneratePlane( List<Pilot> pilots)
    {
        List<string> numbersInUse = new List<string>();
        AirplaneData airplane = airplanes[Random.Range(0, airplanes.Length)];

        foreach(Pilot pilot in OnAir)
            numbersInUse.Add(pilot.Number);
        foreach(Pilot pilot in OnRunway)
            numbersInUse.Add(pilot.Number);
        foreach(Pilot pilot in OnGarage)
            numbersInUse.Add(pilot.Number);
        

        pilots.Add(new Pilot( airplane, numbersInUse, pilotsName));
    }
}
