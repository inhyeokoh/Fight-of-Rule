using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementNav : MonoBehaviour
{
    NavMeshAgent agent;

    public ParticleSystem mouseClickEffect;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);

                mouseClickEffect.transform.position = hit.point;
                mouseClickEffect.Play();
            }
        }
    }
}
