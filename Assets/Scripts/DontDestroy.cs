using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector] public string objectID;
    private void Awake()
    {
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }

    void Start()
    {
        foreach (DontDestroy dontDestroyObject in FindObjectsOfType<DontDestroy>().ToList())
        {
            if (dontDestroyObject != this && dontDestroyObject.objectID == objectID)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}
