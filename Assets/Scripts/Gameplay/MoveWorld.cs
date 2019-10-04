using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWorld : MonoBehaviour
{
    void OnDisable()
    {
        PlayerPawn.OnMoveWorld -= PlayerPawn_OnMoveWorld;

    }

    void OnEnable()
    {
        PlayerPawn.OnMoveWorld += PlayerPawn_OnMoveWorld;
    }

    private void PlayerPawn_OnMoveWorld(float _val1)
    {
        transform.position += Vector3.back * _val1;

    }
}
