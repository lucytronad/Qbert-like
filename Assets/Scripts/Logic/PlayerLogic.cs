using UnityEngine;
using System.Collections;

public class PlayerLogic {

    private Vector3 m_initialPosition;
    public Vector3 M_InitialPosition
    {
        get { return m_initialPosition; }
        set { m_initialPosition = value; }
    }

    private CubeLogic m_cubeLogic;
    public CubeLogic M_CubeLogic
    {
        get { return m_cubeLogic; }
        set { m_cubeLogic = value; }
    }

    public PlayerLogic(Vector3 position, CubeLogic cubeLogic)
    {
        m_initialPosition = position;
        m_cubeLogic = cubeLogic;
    }

    public void Jump(CubeLogic cubeLogic)
    {
        m_cubeLogic = cubeLogic;
        m_cubeLogic.IsReached();
    }

}
