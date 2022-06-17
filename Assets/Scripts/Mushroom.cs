using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    private float timeToDie=0.25f;
    private Vector3 initialScale;

    void Start()
    {
        StartEnemy();
    }

    public override void StartEnemy()
    {
        base.StartEnemy();
        enemyWidth=0.5f;
        enemyHeight=0.5f;
        enemyHead=0.95f;
        rayOffsetOriginSides=Vector3.zero;
        rayOffsetOriginUpwards= new Vector3(-enemyWidth,enemyHeight,0);
        rayOffsetDirUpw=Vector3.right;
        initialScale=transform.GetChild(0).localScale;        
    }

    void FixedUpdate()
    {
        (direction, enemyDamaged)= FixedUpdateMovement(direction, enemyDamaged, originalPosition, rayOffsetDirUpw);
        if (enemyDamaged)
        {
            StartCoroutine(TotalDeath());
        }
    }

    IEnumerator TotalDeath()
    {
        float elapsedTime=0;
        Vector3 currentPosition=transform.position;
        while (elapsedTime<timeToDie)
        {
            float t=elapsedTime/timeToDie;
            transform.GetChild(0).localScale=Vector3.Lerp(initialScale, new Vector3(1,0.2f,1), t*t*50);
            transform.position= Vector3.Lerp(currentPosition, currentPosition-Vector3.up*0.375f,t*t*50);
            elapsedTime+=Time.deltaTime;
            yield return 0;           
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(transform.gameObject);
    }
}
