using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPathDebug : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent m_Agent;

    private LineRenderer m_LineRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        m_LineRenderer = GetComponent < LineRenderer >();
    }

    // Update is called once per frame
    void Update()
    {
        if ( m_Agent.hasPath )
        {
            m_LineRenderer.positionCount = m_Agent.path.corners.Length;
            m_LineRenderer.SetPositions( m_Agent.path.corners );
            m_LineRenderer.enabled = true;
        }
        else
        {
            m_LineRenderer.enabled = false;
        }
    }
}
