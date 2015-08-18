using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class GameLogic {

    private bool m_reverseMode;
    public bool M_ReverseMode
    {
        get { return m_reverseMode; }
        set { m_reverseMode = value; }
    }

    private Vector3 m_cameraInitialPosition;
    public Vector3 M_CameraInitialPosition
    {
        get { return m_cameraInitialPosition; }
    }

    private Vector3 m_cameraInitialRotation;
    public Vector3 M_CameraInitialRotation
    {
        get { return m_cameraInitialRotation; }
    }

    private int m_touchCount;
    public int M_TouchCount
    {
        get { return m_touchCount; }
    }

    private List<Material> m_cubeMaterials;
    public List<Material> M_CubeMaterial
    {
        get { return m_cubeMaterials; }
        set { m_cubeMaterials = value; }
    }

    private List<CubeLogic> m_cubeLogics;
    public List<CubeLogic> M_CubeLogics
    {
        get { return m_cubeLogics; }
        set { m_cubeLogics = value; }
    }

    private PlayerLogic m_playerLogic;
    public PlayerLogic M_PlayerLogic
    {
        get { return m_playerLogic; }
        set { m_playerLogic = value; }
    }

    private List<EnemyLogic> m_enemyLogics;
    public List<EnemyLogic> M_EnemyLogics
    {
        get { return m_enemyLogics; }
        set{ m_enemyLogics = value; }
    }

    public GameLogic(string xmlPathFile)
    {
        m_cubeLogics = new List<CubeLogic>();
        LoadLevelFromXml(xmlPathFile);
        Debug.Log(ToString());
    }

    public void LoadLevelFromXml(string xmlPathFile)
    {
        TextAsset xmlData = (TextAsset)Resources.Load(xmlPathFile, typeof(TextAsset));
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlData.text);
        XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Level");

        if (nodeList.Count == 1)
        {
            XmlNode node = nodeList.Item(0);
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "ReverseMode")
                {
                    m_reverseMode = XmlConvert.ToBoolean(child.InnerText);
                }
                if (child.Name == "CameraInitialPosition")
                {
                    m_cameraInitialPosition = Utility.GetNodePosition(child);
                }
                if (child.Name == "CameraInitialRotation")
                {
                    m_cameraInitialRotation = Utility.GetNodePosition(child);
                }
                if (child.Name == "TouchCount")
                {
                    m_touchCount = m_reverseMode ? 1 : Utility.ParseStringToInt(child.InnerText);
                }
                if (child.Name == "ArrayOfCubeMaterials")
                {
                    m_cubeMaterials = new List<Material>();
                    foreach (XmlNode material in child.ChildNodes)
                    {
                        m_cubeMaterials.Add((Material)Resources.Load("Materials/" + material.InnerText, typeof(Material)));
                    }
                }
                if(child.Name == "ArrayOfCubes")
                {
                    m_cubeLogics = new List<CubeLogic>();
                    foreach (XmlNode cube in child.ChildNodes)
                    {
                        if(cube.Name == "Cube")
                        {
                            m_cubeLogics.Add(new CubeLogic(this, Utility.GetNodePosition(cube), m_cubeMaterials[0]));
                        }
                    }
                }
                if (child.Name == "ArrayOfNeighbors")
                {
                    for (int i = 0; i < child.ChildNodes.Count; i++)
                    {
                        if (child.ChildNodes[i].Name == "Neighbors")
                        {
                            foreach(XmlNode neighbor in child.ChildNodes[i])
                            {
                                int neighborIndex = Utility.ParseStringToInt(neighbor.InnerText);
                                if(neighborIndex >= 0 && neighborIndex < m_cubeLogics.Count)
                                {
                                    m_cubeLogics[i].AddNeighbor(m_cubeLogics[neighborIndex]);
                                }
                                else
                                {
                                    Debug.LogError("Neighbor index is not valid.");
                                }
                                
                            }
                        }
                    }
                }
                if (child.Name == "ArrayOfEnemies")
                {
                    m_enemyLogics = new List<EnemyLogic>();
                    foreach (XmlNode enemy in child.ChildNodes)
                    {
                        if (enemy.Name == "Enemy")
                        {
                            m_enemyLogics.Add(new EnemyLogic(Utility.GetNodePosition(enemy), m_cubeLogics[Utility.ParseStringToInt(enemy.InnerText)]));
                        }
                    }
                }
                if (child.Name == "PlayerInitialPosition")
                {
                    m_playerLogic = new PlayerLogic(Utility.GetNodePosition(child), m_cubeLogics[Utility.ParseStringToInt(child.InnerText)]);
                }
            }
        }
    }

    public override string ToString() 
    {
        string log = "Reverse Mode : " + m_reverseMode + "\n";
        log += "Camera Initial Position: " + m_cameraInitialPosition.ToString() + "\n";
        log += "Camera Initial Rotation: " + m_cameraInitialRotation.ToString() + "\n";
        log += "Player Initial Position: " + m_playerLogic.M_InitialPosition.ToString() + "\n";
        log += "Touch Count: " + m_touchCount + "\n";
        log += "Number Of Level Blocks: " + m_cubeLogics.Count + "\n";
        return log;
    }

    public bool IsLevelComplete()
    {
        foreach(CubeLogic cubeLogic in m_cubeLogics)
        {
            if(cubeLogic.M_TouchCount<m_touchCount)
            {
                return false;
            }
        }
        return true;
    }

}