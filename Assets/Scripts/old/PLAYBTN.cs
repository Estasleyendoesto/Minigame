using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYBTN : MonoBehaviour
{
    public void RESUME()
    {
        Time.timeScale = 1;
        GameObject.Find("PAUSEGUI").SetActive(false);
        GameObject.Find("audiopause").GetComponent<AudioSource>().Play();
    }
}
