using UnityEngine;
using System.Collections;

public class SpawnerLogic {

    private Vector3 m_position;
    public Vector3 M_Position
    {
        get { return m_position; }
    }

    public SpawnerLogic(Vector3 position) 
    {
        m_position = position;
    }

}
