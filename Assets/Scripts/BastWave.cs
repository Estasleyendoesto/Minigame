using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BastWave : MonoBehaviour
{
    public int pointsCount;
    public float maxRadius;
    public float speed;
    public float startWidth;

    private LineRenderer lineRenderer;

    private float time;
    public float maxtimes;
    public float interval;
    private bool exploooosion;
    private float timescounter;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointsCount + 1;

        exploooosion = true;
        timescounter = 0;
    }

    void Update()
    {
        if (timescounter >= maxtimes)
            return;

        if (exploooosion)
        {
            exploooosion = false;
            timescounter++;

            StartCoroutine(Blast());
        }
        else
        {
            if (time < interval)
                time += Time.deltaTime;
            else
            {
                time = 0;
                exploooosion = true;    
            }
        }


        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     StartCoroutine(Blast());
        // }
    }

    IEnumerator Blast()
    {
        float currentRadius = 0;
        while(currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
            yield return null;
        }
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / pointsCount;

        for (int i=0; i<= pointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }
}
