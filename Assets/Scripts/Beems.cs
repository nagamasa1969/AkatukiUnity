using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beems : MonoBehaviour
{
    public GameObject beem;
    public ScrollObject transformy;
    public float startPosition;

    public void OnScrollEnd()
    {
        Vector3 restartPosition = transform.position;
        //restartPosition.y = Random.Range(0f, 2.5f);
        //Debug.Log(restartPosition.y);
        transform.position = restartPosition;
    }
}