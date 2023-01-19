using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScenePilot : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private float normalSpeed;
    [SerializeField]
    private SpriteRenderer airplane;
    public bool InTarget{ get => agent.remainingDistance < 1f; }

    public void GoToTraget(Vector3 pos)
    {
        NavMeshHit navmeshPos = new NavMeshHit();
        NavMesh.SamplePosition(pos, out navmeshPos, 10f, NavMesh.AllAreas);
        agent.SetDestination(navmeshPos.position);
        NavMeshPath path = new NavMeshPath();
            
    }
    private void Awake()
    {
        StartCoroutine(Movement());
    }


    private IEnumerator Movement()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                yield return StartCoroutine(NormalSpeed(agent));
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    private IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, data.endPos, agent.speed * Time.deltaTime);
            yield return null;
        }
    }

    public void SetAirplaneSprite(Sprite sprite)
    {
        airplane.sprite = sprite;
    }
}
