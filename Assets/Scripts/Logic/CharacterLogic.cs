using UnityEngine;
using System.Collections;

public abstract class CharacterLogic {
    protected Vector3 m_initialPosition;
    public Vector3 M_InitialPosition
    {
        get { return m_initialPosition; }
        set { m_initialPosition = value; }
    }

    protected CubeLogic m_cubeLogic;
    public CubeLogic M_CubeLogic
    {
        get { return m_cubeLogic; }
        set { m_cubeLogic = value; }
    }
    
    public CharacterLogic(Vector3 position, CubeLogic cubeLogic)
    {
        m_initialPosition = position;
        m_cubeLogic = cubeLogic;
    }

    public virtual void Jump(CubeLogic cubeLogic)
    {
        m_cubeLogic = cubeLogic;
    }
}
