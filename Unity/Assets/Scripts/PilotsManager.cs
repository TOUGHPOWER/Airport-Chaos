using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Random = UnityEngine.Random;
using TMPro;
using System.Linq;

public class PilotsManager : MonoBehaviour
{

    private const float START_SHIFT = 20;
    private const float FINISH_SHIFT = 5;
    private const int PILOT_CALL_STRIKES = 3;

    [InfoBox("Tmer in seconds")]
    [SerializeField] private float ShiftTimer;
    [SerializeField] private float timeWaitingOnCall;
    [SerializeField] private float timeBetweenCalls;
    public float Score { get => ((totalPilotsToday - missedCalls) / totalPilotsToday) * 100; }
    private int missedCalls;
    private int totalPilotsToday;
    public Pilot PilotOnCall { get; set; }

    [SerializeField] private AirplaneData[] airplanes;
    [SerializeField] private string[] pilotsFirstName;
    [SerializeField] private string[] pilotsLastName;
    [SerializeField] private Location[] Runways;
    [SerializeField] private Location[] Garages;

    [SerializeField, InfoBox("X:Time in hours(0.01 = 1h)\nY:Spawn rate in units per 20 min(0.1 = 1 unit)")]
    private AnimationCurve rushHours;

    [field: SerializeField, ReadOnly] public List<Pilot> OnAir { get; private set; }
    [field: SerializeField, ReadOnly] public List<Pilot> OnRunway { get; private set; }
    [field: SerializeField, ReadOnly] public List<Pilot> OnGarage { get; private set; }
    [field: SerializeField, ReadOnly] public List<Pilot> Calling { get; private set; }
    [SerializeField] private TextMeshProUGUI clock;

    public float ShiftDuration
    {
        get
        {
            if (START_SHIFT < FINISH_SHIFT)
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
            float currentTime = (START_SHIFT) + ((timer * ShiftDuration) / ShiftTimer);

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
        missedCalls = 0;
        totalPilotsToday = 0;
        PilotOnCall = null;
        Calling = new List<Pilot>();

        int pilotsInAirport = Random.Range(0, Garages.Length);


        /*for(int i = 0; i< pilotsInAirport; i++)
        {
            GeneratePlane(OnGarage);

            int garage = 0;
            do
            {
                garage = Random.Range(0, Garages.Length);
            }while(Garages[garage].IsOccupied);

            Garages[garage].TryToMovePilotIn(OnGarage[OnGarage.Count-1]);
        }*/

        StartCoroutine(Spawn());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (CurrentRealTime.Hours == FINISH_SHIFT)
        {
            FinishShift();
        }
        string s = string.Format("{0,2:D2}:{1,2:D2}", CurrentRealTime.Hours, CurrentRealTime.Minutes);
        if (clock != null)
            clock.text = s;
    }

    private void FinishShift()
    {
        Debug.Log("Finish");
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(((ShiftTimer / ShiftDuration) / 3) / SpawnRate);
            GeneratePlane(OnAir);
            print("Spawn: " + OnAir[OnAir.Count - 1].Number);
        }
    }

    private void GeneratePlane(List<Pilot> pilots)
    {
        List<string> numbersInUse = new List<string>();
        AirplaneData airplane = airplanes[Random.Range(0, airplanes.Length)];

        foreach (Pilot pilot in OnAir)
            numbersInUse.Add(pilot.Number);
        foreach (Pilot pilot in OnRunway)
            numbersInUse.Add(pilot.Number);
        foreach (Pilot pilot in OnGarage)
            numbersInUse.Add(pilot.Number);
        numbersInUse.Add("1000");
        numbersInUse.Add("2000");
        numbersInUse.Add("3000");
        numbersInUse.Add("4000");

        totalPilotsToday++;
        pilots.Add(new Pilot(airplane, numbersInUse, pilotsFirstName, pilotsLastName));
        StartCoroutine(Call(pilots.Last()));
    }

    private IEnumerator Call(Pilot pilot)
    {
        Calling.Add(pilot);
        for (int i = 0; i < PILOT_CALL_STRIKES; i++)
        {
            Calling.Add(pilot);
            yield return new WaitForSeconds(timeWaitingOnCall);
            if (Calling.Contains(pilot) && PilotOnCall != pilot)
                Calling.Remove(pilot);
            else
                yield break;
            yield return new WaitForSeconds(timeBetweenCalls);
        }

        DefaultAction(pilot);
    }

    private void DefaultAction(Pilot pilot)
    {
        switch (pilot.Need)
        {
            case Request.Land:
                //leaves
                RemovePilotFromGame(pilot);
                break;
            case Request.TakeOff:
                //takes off
                break;
            case Request.Park:
                //parks
                break;
            default:
                break;
        }

        missedCalls++;
    }

    private void Crash(Pilot p1, Pilot p2)
    {
        RemovePilotFromGame(p1);
        RemovePilotFromGame(p2);
    }

    private void RemovePilotFromGame(Pilot pilot)
    {
        if(Calling.Contains(pilot))
            Calling.Remove(pilot);
        
        if(OnAir.Contains(pilot))
            OnAir.Remove(pilot);

        if(OnRunway.Contains(pilot))
            OnRunway.Remove(pilot);

        if(OnGarage.Contains(pilot))
            OnGarage.Remove(pilot);
        
        foreach(Location loc in Runways)
        {
            if(loc.PilotInLocation == pilot)
                loc.RemovePilot();
        }

        foreach(Location loc in Garages)
        {
            if(loc.PilotInLocation == pilot)
                loc.RemovePilot();
        }
    }

    public void TryToLand(Pilot pilot)
    {
        if(OnAir.Contains(pilot))
        {
            OnAir.Remove(pilot);
            OnRunway.Add(pilot);
            //will have to wait for physic object
            int runwayToMove = 0;
            for(int i = Runways.Count()-1; i>=0; i--)
            {
                if(!Runways[i].IsOccupied)
                {
                    runwayToMove = i;
                    break;
                }
            }

            (bool canMove, bool crash) = Runways[runwayToMove].TryToMovePilotIn(pilot);

            if(!canMove || crash)
                Crash(pilot, Runways[runwayToMove].PilotInLocation);
        }
    }
}
