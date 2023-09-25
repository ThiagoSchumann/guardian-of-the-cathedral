using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrulharAleatorio : MonoBehaviour
{
    private NavMeshAgent agente;
    public float range;
    public float tempo;

    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame

    public void Patrulhar()
    {
        if (agente.remainingDistance <= agente.stoppingDistance || (tempo > 6.0f))
        {
            Vector3 point;
            if (RandomPoint(transform.position, range, out point))
            {
                agente.SetDestination(point);
                tempo = 0;
            }
        }
        else
        {
            tempo += Time.deltaTime;
        }

        Debug.DrawLine(transform.position, agente.destination, Color.magenta);

    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        else
        {
            result = Vector3.zero;
            return false;
        }
    }

}
