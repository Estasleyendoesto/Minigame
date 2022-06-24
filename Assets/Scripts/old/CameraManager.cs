using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineFramingTransposer framingTransposer;
    private Rigidbody rb;
    private GameObject onion;
    private GameObject onionModel;
    private int playerRotation;
    private float offset=0;

    void Start()
    {
        onion=GameObject.Find("Onion");
        rb=onion.GetComponent<Rigidbody>();
        onionModel=GameObject.Find("OnionModel");
    
        framingTransposer=virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void FixedUpdate()
    {
        playerRotation= GetPlayerRotation();   
        ChangeCameraPosition(playerRotation); 
    }


    public void ChangeCameraPosition(float plaRotation) //se cambia el Tracked Object Offset de la camara virtual
    {
        if ((plaRotation!= 90 && plaRotation!= 270) || (rb.velocity.x<=0.05f && rb.velocity.x>=-0.05f)) //Mientras se gira y no se mueve, la camara no se mueve
            return;
                
        if (rb.velocity.x>0.05f)
        {
            if(offset<3.49)
            {
                //offset+=((rb.velocity.x/150)*(tiSaved2-tiSaved1)); //Codigo para que el movimiento de la camara sea expoencial (ligado al tiempo transcurrido)
                offset+=(rb.velocity.x/100);
            }
            else
                offset=3.5f;
        }
        else if (rb.velocity.x<-0.05f) //el if no haria falta, es eso directamente 
        {
            if(offset>-3.49)
            {
                //offset+=((rb.velocity.x/150)*(tiSaved2-tiSaved1)); //Codigo para que el movimiento de la camara sea expoencial (ligado al tiempo transcurrido)
                offset+=(rb.velocity.x/100);
            }
            else
                offset=-3.5f;
        }
        framingTransposer.m_TrackedObjectOffset.x=offset;
    }

    
    public int GetPlayerRotation()
    {
        playerRotation=Mathf.RoundToInt(onionModel.transform.eulerAngles.y);
        return playerRotation;
    }
}

/*  CLASE CON EL MOVIMIENTO DE LA CAMARA EN FUNCION DEL TIEMPO Y PREPARADO PARA SACAR LAS POSICIONES DE PLAYER
public class Camera : MonoBehaviour
{
    public float cameraHeight=4.5f;
    public float cameraWidth=-17f;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineFramingTransposer framingTransposer;
    private Rigidbody rb;
    private GameObject onion;
    private GameObject onionModel;
    private int playerDirection=0; 
    private int lastPlayerDirection; 
    private float playerPosition;
    private float lastPlayerPosition;
    private int playerRotation;
    private Vector3 cameraPosition;

    private float offset=0;
    private bool directionChanged; 
    private float timeSaved1; 
    private float timeSaved2; 


    void Start()
    {
        onion=GameObject.Find("Onion");
        rb=onion.GetComponent<Rigidbody>();
        onionModel=GameObject.Find("OnionModel");
    
        framingTransposer=virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        cameraPosition.x= framingTransposer.m_TrackedObjectOffset.x;
        cameraPosition.y= cameraHeight;
        cameraPosition.z= cameraWidth;
    }

    void FixedUpdate()
    {
        playerRotation= GetPlayerRotation();   
        (playerDirection,lastPlayerDirection)= GetPlayerDirection(playerDirection);
        (directionChanged,timeSaved1,timeSaved2)= CheckDirectionsChanges(playerDirection,lastPlayerDirection,timeSaved1,timeSaved2);

        ChangeCameraPosition(playerRotation, cameraPosition, directionChanged, timeSaved1, timeSaved2); 
    }


    public void ChangeCameraPosition(float plaRotation, Vector3 camPosition, bool dirChanged, float tiSaved1, float tiSaved2) //se cambia el Tracked Object Offset de la camara virtual
    {
        if ((plaRotation!= 90 && plaRotation!= 270) || (rb.velocity.x<=0.05f && rb.velocity.x>=-0.05f))
            return;
                
        if (rb.velocity.x>0.05f)
        {
            if(offset<3.49)
            {
                //offset+=((rb.velocity.x/150)*(tiSaved2-tiSaved1)); //Codigo para que el movimiento de la camara sea expoencial (ligado al tiempo transcurrido)
                offset+=(rb.velocity.x/100);
            }
            else
                offset=3.5f;
        }
        else if (rb.velocity.x<-0.05f) //el if no haria falta, es eso directamente 
        {
            if(offset>-3.49)
            {
                //offset+=((rb.velocity.x/150)*(tiSaved2-tiSaved1)); //Codigo para que el movimiento de la camara sea expoencial (ligado al tiempo transcurrido)
                offset+=(rb.velocity.x/100);
            }
            else
                offset=-3.5f;
        }
        framingTransposer.m_TrackedObjectOffset.x=offset;
    }


    public (int, int) GetPlayerDirection(int pDirection)
    {
        lastPlayerDirection=pDirection;
        if (rb.velocity.x>0)
            pDirection=1;
        else if (rb.velocity.x<0)
            pDirection=-1;
        else 
            pDirection=0;    

        return (pDirection, lastPlayerDirection);
    }

    public (bool, float, float) CheckDirectionsChanges(int pDirection, int lastPDirection, float tiSaved1, float tiSaved2)
    {
        if(pDirection!=lastPDirection)
        {
            directionChanged=true;
            tiSaved1=tiSaved2;
            tiSaved2=Time.time;
        }
        else
        {
            directionChanged=false;
            tiSaved2=Time.time;
        }

        return (directionChanged, tiSaved1, tiSaved2);
    }


    public float GetPlayerPosition()
    {
        //Calculo de la ultima posicion del jugador iria aqui
        playerPosition=rb.position.x;
        return playerPosition;
    }
    public int GetPlayerRotation()
    {
        playerRotation=Mathf.RoundToInt(onionModel.transform.eulerAngles.y);
        return playerRotation;
    }
}
*/
