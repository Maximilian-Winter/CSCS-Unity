using UnityEngine;
using UnityEngine.AI;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public class NavMeshPathDebug : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent m_Agent;

    private LineRenderer m_LineRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        m_LineRenderer = GetComponent < LineRenderer >();
    }

    // Update is called once per frame
    private void Update()
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

}
