using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamera : MonoBehaviour
{
    public float initY = 3.0f;
    public float offsetY = 2.5f;
    public int sitFrame = 30;
    public int headUpFrame = 30;
    public int changeFrame = 15;

    private CinemachineCameraOffset offComp;
    private Player player;
    private DiscreteGraph changeGraph;
    private int dir = 0;
    private int currentChangeFrame;
    private Vector3 offsetVector;
    private float dx, dy, dz;

    void Start()
    {
        offComp = GetComponent<CinemachineCameraOffset>();
        player = ManagerHub.GetPlayerSceneManager().player;
        offsetVector = Vector3.zero;

        changeGraph = new DiscreteParabolaGraph(changeFrame);
    }

    void FixedUpdate()
    {
        dir = player.proceedSitFrame >= sitFrame ? -1 : 0;
        dir += player.proceedHeadUpFrame >= headUpFrame ? 1 : 0;

        if(dir == 0 && currentChangeFrame > 0)
            currentChangeFrame--;
        else if(dir != 0 && currentChangeFrame < changeFrame - 1)
            currentChangeFrame++;

        dx = 0.0f;
        dy = initY + offsetY * dir * changeGraph[currentChangeFrame];
        dz = -10.0f;

        offsetVector.Set(dx, dy, dz);

        offComp.m_Offset = offsetVector;
    }
}
