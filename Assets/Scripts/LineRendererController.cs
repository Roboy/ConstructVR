using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineRendererController : Singleton<LineRendererController>
{
    [Header("Attributes")]
    [Range(0.2f, 0.8f)]
    public float LineThickness;
    public bool NormalizedVector = false;

    [Header("References")]
    public Transform Origin;
    public Transform Target;
    [SerializeField]
    private LineRenderer m_LineRenderer;

    [Header("Positions")]
    [SerializeField]
    private Vector3 m_OriginPosition;
    [SerializeField]
    private Vector3 m_TargetPosition;
    [SerializeField]
    private Vector3 m_DirectionVector;

    private Vector3 m_PrevOriginPostion;
    private Vector3 m_PrevTargetPosition;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdatePositions();

        if ((m_OriginPosition != m_PrevOriginPostion) || (m_TargetPosition != m_PrevTargetPosition))
        {
            UpdateDrawnLine();
            
        }
        
    }

    private void Initialize()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_OriginPosition = m_PrevOriginPostion = Origin.position;
        m_TargetPosition = m_PrevTargetPosition = Target.position;
        m_DirectionVector = CalculateVector(m_OriginPosition, m_TargetPosition);

        m_LineRenderer.SetPosition(0, m_OriginPosition);
        m_LineRenderer.SetPosition(1, m_TargetPosition);
        m_LineRenderer.startWidth = m_LineRenderer.endWidth = LineThickness;
    }

    private void UpdateDrawnLine()
    {
        m_LineRenderer.SetPosition(0, m_OriginPosition);
        m_LineRenderer.SetPosition(1, m_TargetPosition);
        m_LineRenderer.startWidth = m_LineRenderer.endWidth = LineThickness;

        m_PrevOriginPostion = m_OriginPosition;
        m_PrevTargetPosition = m_TargetPosition;
        

    }

    private void UpdatePositions()
    {
        m_OriginPosition = Origin.position;
        m_TargetPosition = Target.position;
        m_DirectionVector = CalculateVector(m_OriginPosition, m_TargetPosition);
    }

    private Vector3 CalculateVector(Vector3 origin, Vector3 target)
    {
        Vector3 result = Vector3.zero;
        result = target - origin;

        if (NormalizedVector)
        {
            result = Vector3.Normalize(result);
        }

        return result;
    }
}
