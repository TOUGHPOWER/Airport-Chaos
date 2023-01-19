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


    private const float OFFSET = 10;
    private const int PILOT_CALL_STRIKES = 3;
    private const int MAX_CRASH_STRIKES = 3;
    [SerializeField] private float startShift = 20;
    [SerializeField] private float finishShift = 5;
    [InfoBox("Tmer in seconds")]
    [SerializeField] private float ShiftTimer;
    [SerializeField] private float timeWaitingOnCall;
    [SerializeField] private float timeBetweenCalls;
    [SerializeField] private float timeInRunWay;
    [SerializeField] private float timeInGarage;
    public int Score { get => totalRequests * 3 - missedCalls; }
    public float ScorePercentil { get => Score / totalRequests * 100; }
    private int missedCalls;
    private int totalRequests;
    private int strikes;
    public Pilot PilotOnCall { get; set; }
    [SerializeField]
    private bool testing;
    [SerializeField] private ScenePilot pilotPrefab;
    [SerializeField] private GameObject explotionPrefab;
    [SerializeField] private UIManager ui;
    [SerializeField] private Transform takeOffLocation;
    [SerializeField] private Transform startLocation;
    [SerializeField] private AirplaneData[] airplanes;
    [SerializeField] private string[] pilotsFirstName;
    [SerializeField] private string[] pilotsLastName;
    [SerializeField] private Location[] Runways;
    [SerializeField, ReadOnly] private bool[] runwayAvailables;
    [SerializeField] private Location[] Garages;
    [SerializeField, ReadOnly] private bool[] garageAvailables;

    [SerializeField, InfoBox("X:Time in hours(0.01 = 1h)\nY:Spawn rate in units per 20 min(0.1 = 1 unit)")]
    private AnimationCurve rushHours;

    [field: SerializeField, ReadOnly] public List<Pilot> OnAir { get; private set; }
    [field: SerializeField, ReadOnly] public List<Pilot> OnRunway { get; private set; }
    [field: SerializeField, ReadOnly] public List<Pilot> OnGarage { get; private set; }
    [field: SerializeField, ReadOnly] public List<Pilot> Calling { get; private set; }
    [SerializeField] private TextMeshProUGUI clock;
    [SerializeField, ShowIf("testing"), Button]
    private void Call() => GeneratePlane(OnAir, startLocation);
    public bool FinishGame{ get; private set; }

    public float ShiftDuration
    {
        get
        {
            if (startShift < finishShift)
                return finishShift - startShift;
            else
            {
                return (24 - startShift) + finishShift;
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
            float currentTime = (startShift) + ((timer * ShiftDuration) / ShiftTimer);

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
        runwayAvailables = new bool[Runways.Length];
        garageAvailables = new bool[Garages.Length];
        for (int i = 0; i < runwayAvailables.Length; i++)
            runwayAvailables[i] = true;
        for (int i = 0; i < garageAvailables.Length; i++)
            garageAvailables[i] = true;
        timer = 0;
        missedCalls = 0;
        totalRequests = 0;
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
        if (!testing)
            StartCoroutine(Spawn());
    }

    void Update()
    {
        if (strikes >= MAX_CRASH_STRIKES && !FinishGame)
        {
            FinishShift();
        }
        timer += Time.deltaTime;
        if (CurrentRealTime.Hours >= finishShift && !FinishGame)
        {
            FinishShift();
        }
        string s = string.Format("{0,2:D2}:{1,2:D2}", CurrentRealTime.Hours, CurrentRealTime.Minutes);
        ui.UpdateClock(s);

    }

    private void FinishShift()
    {
        StopAllCoroutines();
        FinishGame = true;
        int day = PlayerPrefs.GetInt("PlayerDay", 1);

        if(strikes < MAX_CRASH_STRIKES)
            day++;
        else
            day = 1;

        PlayerPrefs.SetInt("PlayerDay", day);

        string evaluation = (ScorePercentil > 16) ? (ScorePercentil > 32) ?
            (ScorePercentil > 49) ? (ScorePercentil > 66) ? (ScorePercentil > 82) ?
            "A" : "B" : "C" : "D" : "E" : "F";

        ui.ShowEvaluation(Score, day, evaluation);
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(((ShiftTimer / ShiftDuration) / 3) / SpawnRate);
            GeneratePlane(OnAir, startLocation);
            print("Spawn: " + OnAir[OnAir.Count - 1].Number);
        }
    }

    private void GeneratePlane(List<Pilot> pilots, Transform spawnTransform)
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

        totalRequests++;
        pilots.Add(new Pilot(airplane, numbersInUse, pilotsFirstName, pilotsLastName, pilotPrefab, spawnTransform.position));
        StartCoroutine(Call(pilots.Last()));
    }

    private IEnumerator Call(Pilot pilot)
    {
        Request initialNeed = pilot.Need;

        for (int i = 0; i < PILOT_CALL_STRIKES; i++)
        {
            yield return new WaitUntil(() => Calling.Count < 5);

            Calling.Add(pilot);

            GameObject popUp = ui.CreatePopUp("ID: " + pilot.Number +
                $"\nMissed Calls: " + i);

            Coroutine c = StartCoroutine(Wait(timeWaitingOnCall));

            yield return new WaitUntil(() => (c == null || !Calling.Contains(pilot)));

            ui.RemovePopUp(popUp);

            if (c != null)
                StopCoroutine(c);

            if (Calling.Contains(pilot) && PilotOnCall != pilot)
                Calling.Remove(pilot);
            else if (initialNeed != pilot.Need)
                yield break;

            missedCalls++;
            yield return new WaitForSeconds(timeBetweenCalls);
        }

        DefaultAction(pilot);
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void DenyRequest(Pilot pilot)
    {
        Calling.Remove(pilot);
        missedCalls++;
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
                TryToTakeOf(pilot);
                break;
            case Request.Park:
                TryToPark(pilot, 0);
                break;
            default:
                break;
        }

        IncriseStrikes();
    }

    private void Crash(Pilot p1, Pilot p2)
    {
        print("crash");
        if (explotionPrefab != null)
            Instantiate(explotionPrefab, p1.PilotInScene.transform.position, p1.PilotInScene.transform.rotation);
        RemovePilotFromGame(p1);

        if (explotionPrefab != null)
            Instantiate(explotionPrefab, p2.PilotInScene.transform.position, p2.PilotInScene.transform.rotation);
        RemovePilotFromGame(p2);

        IncriseStrikes();
    }
    private void Crash(Pilot p1)
    {
        if (explotionPrefab != null)
            Instantiate(explotionPrefab, p1.PilotInScene.transform.position, p1.PilotInScene.transform.rotation);
        RemovePilotFromGame(p1);

        IncriseStrikes();
    }

    private void IncriseStrikes()
    {
        missedCalls += 6;
        strikes++;
        ui.IncreaseStrikes(strikes);
    }

    private void RemovePilotFromGame(Pilot pilot)
    {
        Destroy(pilot.PilotInScene.gameObject);
        if (Calling.Contains(pilot))
            Calling.Remove(pilot);

        if (OnAir.Contains(pilot))
            OnAir.Remove(pilot);

        if (OnRunway.Contains(pilot))
            OnRunway.Remove(pilot);

        if (OnGarage.Contains(pilot))
            OnGarage.Remove(pilot);

        RemoveFromLocation(Runways, pilot, runwayAvailables);
        RemoveFromLocation(Garages, pilot, garageAvailables);
    }

    private void RemoveFromLocation(Location[] locations, Pilot pilot, bool[] avalables)
    {
        int delete = 0;
        for (int i = 0; i < locations.Length; i++)
            if (locations[i].PilotInLocation == pilot)
                delete = i;

        locations[delete].RemovePilot();
        avalables[delete] = true;

    }

    public void TryToLand(Pilot pilot)
    {
        if (OnAir.Contains(pilot))
        {
            OnAir.Remove(pilot);
            OnRunway.Add(pilot);
            //will have to wait for physic object
            int runwayToMove = 0;
            for (int i = Runways.Count() - 1; i >= 0; i--)
            {
                print(Runways[i].Name + " is ocupied: " + runwayAvailables[i]);
                if (runwayAvailables[i])
                {
                    runwayToMove = i;
                    break;
                }
            }

            pilot.Need = Request.Park;
            StartCoroutine(MovePlane(Runways, pilot, runwayToMove, runwayAvailables));
        }
    }

    public void TryToTakeOf(Pilot pilot)
    {
        if (OnGarage.Contains(pilot))
        {
            OnGarage.Remove(pilot);

            pilot.Need = Request.Nothing;
            RemoveFromLocation(Garages, pilot, garageAvailables);
            StartCoroutine(MovePlane(pilot));
        }
    }

    public void TryToPark(Pilot pilot, int i)
    {
        if (OnRunway.Contains(pilot))
        {
            OnRunway.Remove(pilot);
            OnGarage.Add(pilot);

            RemoveFromLocation(Runways, pilot, runwayAvailables);
            pilot.Need = Request.TakeOff;
            StartCoroutine(MovePlane(Garages, pilot, i, garageAvailables));
        }

    }

    private IEnumerator MovePlane(Location[] locations, Pilot pilot, int index, bool[] avalables)
    {
        bool crash = !avalables[index];
        bool canMove = false;
        if (!crash)
            canMove = locations[index].TryToMovePilotIn(pilot);

        avalables[index] = false;
        pilot.PilotInScene.GoToTraget(locations[index].Position.position);

        print("can move: " + canMove);
        print("crash: " + crash);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => pilot.PilotInScene.InTarget);
        print("arrived");
        if (crash)
            Crash(pilot, locations[index].PilotInLocation);
        else if (!canMove)
            Crash(pilot);
        else
            StartCoroutine(NextRequest(pilot));
        yield break;
    }

    private IEnumerator NextRequest(Pilot pilot)
    {
        float timeToWait = 0;
        totalRequests++;
        switch (pilot.Need)
        {
            case Request.Park:
                timeToWait = timeInRunWay;
                break;
            case Request.TakeOff:
                timeToWait = timeInGarage;
                break;
            default:
                break;
        }

        timeToWait = Random.Range(timeToWait - OFFSET, timeToWait + OFFSET);
        if (timeToWait < 0)
            timeToWait = 0;

        yield return new WaitForSeconds(timeToWait);

        StartCoroutine(Call(pilot));
    }

    private IEnumerator MovePlane(Pilot pilot)
    {
        //(bool canMove, bool crash) = locations[index].TryToMovePilotIn(pilot);

        pilot.PilotInScene.GoToTraget(takeOffLocation.position);

        /*print("can move: " + canMove);
        print("crash: " + crash);

        if(!canMove || crash)
            Crash(pilot, locations[index].PilotInLocation);*/
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => pilot.PilotInScene.InTarget);
        RemovePilotFromGame(pilot);

        yield break;
    }
}
