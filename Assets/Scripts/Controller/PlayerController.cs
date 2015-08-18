using UnityEngine;
using System.Collections;

public class PlayerController : CharacterController {
	
    public void Initialize(PlayerLogic playerLogic, GameObject currentCube)
    {
        m_characterLogic = playerLogic;
        m_currentCube = currentCube;
    }

}
