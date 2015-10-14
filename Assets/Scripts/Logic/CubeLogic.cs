using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeLogic {

    private GameLogic m_gameLogic;

    private Vector3 m_initialPosition;
    public Vector3 M_InitialPosition
    {
        get{ return m_initialPosition; }
        set{ m_initialPosition = value; }
    }

    private Material m_material;
    public Material M_Material
    {
        get { return m_material; }
        set { m_material = value; }
    }

    private List<CubeLogic> m_neighbors;
    public List<CubeLogic> M_Neighbors
    {
        get { return m_neighbors; }
        set { m_neighbors = value; }
    }

    private int m_touchCount;
    public int M_TouchCount
    {
        get { return m_touchCount; }
        set { m_touchCount = value; }
    }

    public CubeLogic(GameLogic gameLogic, Vector3 position, Material material)
    {
        m_gameLogic = gameLogic;
        m_initialPosition = position;
        m_neighbors = new List<CubeLogic>();
        m_material = material;
        m_touchCount = 0;
    }

    public void AddNeighbor(CubeLogic cubeLogic)
    {
        m_neighbors.Add(cubeLogic);
    }

    public void IsReached()
    {
        m_touchCount++;
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if(m_gameLogic.M_ReverseMode)
        {
            m_material = m_gameLogic.M_CubeMaterial[m_touchCount % (m_gameLogic.M_TouchCount + 1)];
        }
        else
        {
            if(m_touchCount<=m_gameLogic.M_TouchCount)
            {
                m_material = m_gameLogic.M_CubeMaterial[m_touchCount];
            }
        }
    }

    public List<CubeLogic> FindLowerNeighbors()
    {
        List<CubeLogic> lowerNeighbors = new List<CubeLogic>();
        foreach(CubeLogic cubeLogic in m_neighbors)
        {
            if(cubeLogic.M_InitialPosition.y < m_initialPosition.y)
            {
                lowerNeighbors.Add(cubeLogic);
            }
        }

        return lowerNeighbors;
    }

}
