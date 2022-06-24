using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carapace : MonoBehaviour
{
    public GameObject turtle;
    public GameObject carapace;
    private float elapsedTime;
    private float beingCarapaceTime=8;
    private float shakingTime=3;       
    private Vector3 carapaceInitialScale;
    private float carapaceHeight;
    private bool enemyDamaged=false;

    void Start()
    {
        elapsedTime=0; 
        carapaceInitialScale=carapace.transform.GetChild(0).localScale;
        carapaceHeight=0.4f;
        
    }

    void Update()
    {
        elapsedTime=BeingCarapace(elapsedTime, carapace, turtle);
    }

    public float BeingCarapace(float eTime, GameObject carap, GameObject turt)
    {   
        if(eTime<beingCarapaceTime)
        {
            enemyDamaged=CheckDamage(enemyDamaged);
            if(eTime>beingCarapaceTime-shakingTime)
            {
                StartCoroutine(ShakingCarapace(enemyDamaged));
            }
            eTime+=Time.deltaTime;
            if(enemyDamaged&& eTime>1) // Muerte parcial
            {         
                eTime=0;
                ParcialDeath(carap, turt);       
            }   
        }
        else //Vuelve a ser tortuga
        {
            eTime=0;
            Vector3 turtlePosition=carap.transform.position;
            turt.transform.position=turtlePosition;            
            turt.SetActive(true);
            carap.transform.GetChild(0).localScale=carapaceInitialScale;
            carap.SetActive(false);
        } 
        return (eTime);  
    }

    IEnumerator ShakingCarapace(bool eDamaged)
    {
        float elapsedTime=0;
        while (elapsedTime<shakingTime)
        {
            if (eDamaged)
            {
                carapace.transform.GetChild(0).localScale=carapaceInitialScale;
                break;
            }
            float t=elapsedTime/shakingTime;
            carapace.transform.GetChild(0).localScale=Vector3.one*(0.75f+0.25f*Mathf.Abs(Mathf.Sin(t*t*20)));
            elapsedTime+=Time.deltaTime;
            yield return 0;           
        }
        yield return 0;
    }

    public bool CheckDamage(bool eDamaged)
    {    
        Vector3 rayOrigin=transform.position;
        Vector3 rayDirection=Vector3.up;       
        Debug.DrawLine(rayOrigin,rayOrigin+rayDirection*carapaceHeight, Color.yellow);
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo,carapaceHeight)) 
        {
            if (hitInfo.transform.tag =="Player")
            {
                eDamaged=true;
            }
            else eDamaged=false;           
        }
        else
            eDamaged=false;
        return eDamaged;
    }

    public void ParcialDeath(GameObject carap, GameObject turt)
    {
        Vector3 turtlePosition=carap.transform.position+new Vector3(1,0,0)*100; //quiero que la tortuga se active en otro punto, para que se vuelva a mover cuando player se aleje de su pto origen
        turt.transform.position=turtlePosition; 
        turt.SetActive(true); 
        //turt.GetComponent<Turtle>().ActiveAndRecolocateEnemy(dir, false ,turtleOriginalPosition);
        carap.SetActive(false);
    }
}
