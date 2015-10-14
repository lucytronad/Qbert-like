using UnityEngine;
using System.Collections;

public class PlayerLogic : CharacterLogic {

    private bool m_isDead;
    public bool M_IsDead
    {
        get { return m_isDead; }
        set { m_isDead = value; }
    }

    public PlayerLogic(Vector3 position, CubeLogic cubeLogic) : base (position,cubeLogic)
    {
        m_isDead = false;
    }
    

    public override void Jump(CubeLogic cubeLogic)
    {
        base.Jump(cubeLogic);
        cubeLogic.IsReached();
    }
}
