using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static SceneTransitionManager Instance { get; private set; }

    public float LastTransitionTime { get; private set; }

    public void UpdateLastTransitionTime()
    {
        LastTransitionTime = Time.time;
    }

    public bool CanTransition()
    {
        return Time.time - LastTransitionTime >= 0.25f;
    }
}
