using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshRenderers;

    public Room[] roomsAdyasent;

    public Wall[] walls;

    private void Start()
    {
        //SetActiveRoom(false);
    }

    public void SetActiveRoom(bool isActive)
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].enabled = isActive;
        }
    }
}
