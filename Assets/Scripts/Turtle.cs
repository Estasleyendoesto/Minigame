using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Turtle : Enemy
{
    public GameObject turtle;
    public GameObject carapace;

    void Start()
    {   
        StartEnemy();
        carapace.SetActive(false); 
    }

    public override void StartEnemy()
    {
        base.StartEnemy();        
        enemyWidth=0.5f;
        enemyHeight=1f;
        enemyHead=0.5f;
        rayOffsetOriginSides=-Vector3.up*0.5f;
        rayOffsetOriginUpwards= new Vector3(0,enemyHeight,0);
        rayOffsetDirUpw=Vector3.right*direction;    }
    void FixedUpdate()
    { 
        direction=AvoidHoles(direction);
        rayOffsetDirUpw=Vector3.right*direction;
        (direction, enemyDamaged)= FixedUpdateMovement(direction, enemyDamaged, originalPosition, rayOffsetDirUpw);        if (enemyDamaged==true)
        {
            Vector3 carapacePosition=turtle.transform.position;
            carapace.transform.position=carapacePosition;
            turtle.SetActive(false);
            carapace.SetActive(true);                                             
        }        
    }

    public int AvoidHoles(int dir)
    {
        //Rayo al suelo por delante de Onion
        Vector3 forwardRayOrigin=transform.position-Vector3.right*(enemyWidth-0.1f);
        Vector3 forwardRayDirection=-Vector3.up;
        Debug.DrawLine(forwardRayOrigin,forwardRayOrigin+forwardRayDirection*(enemyHeight+0.1f), Color.blue);
        if (!Physics.Raycast(forwardRayOrigin, forwardRayDirection, out RaycastHit forwardHitInfo,enemyHeight+0.1f)) //Si no te chocas con nada, cambia de direccion
            dir= dir*-1; 
        
        //Rayo al suelo por detras de Onion
        Vector3 backwardRayOrigin=transform.position+Vector3.right*(enemyWidth-0.1f);
        Vector3 backwardRayDirection=-Vector3.up;
        Debug.DrawLine(backwardRayOrigin,backwardRayOrigin+backwardRayDirection*(enemyHeight+0.1f), Color.blue);
        if (!Physics.Raycast(backwardRayOrigin, backwardRayDirection, out RaycastHit backwardHitInfo,enemyHeight+0.1f)) //Si no te chocas con nada, cambia de direccion
            dir= dir*-1;

        return dir;
    }
}