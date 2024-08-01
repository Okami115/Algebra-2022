using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Ball[] smooth;
    [SerializeField] private Ball[] scratched;
    [SerializeField] private Ball white;
    [SerializeField] private Ball black;
    [SerializeField] private Hole[] holes;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [SerializeField] private GameObject player1win;
    [SerializeField] private GameObject player2win;

    

    bool smoothMatchPoint = false;
    bool scratchedMatchPoint = false;

    public Action endTurn;

    public bool playerTurn = true;

    public bool wait = false;


    private void OnEnable()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            holes[i].blackInHole += CheckWinCondition;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            holes[i].blackInHole -= CheckWinCondition;
        }
    }

    private void Update()
    {
        CheckMatchPoint();
        CheckEndTurn();
    }

    private void CheckMatchPoint()
    {
        int iterado = 0;
        for (int i = 0; i < smooth.Length; i++)
        {
            if (!smooth[i].isActive)
            {
                iterado++;
                break;
            }
        }

        if (iterado == smooth.Length)
            smoothMatchPoint = true;

        iterado = 0;

        for (int i = 0; i < scratched.Length; i++)
        {
            if (!scratched[i].isActive)
            {
                iterado++;
                break;
            }
        }

        if (iterado == scratched.Length)
            scratchedMatchPoint = true;
    }

    private void CheckEndTurn()
    {
        if(wait)
        {
            int iterado = 0;
            for (int i = 0; i < smooth.Length; i++)
            {
                if (Vector3.Distance(smooth[i].velocity, Vector3.zero) < Vector3.kEpsilon)
                {
                    iterado++;
                }
            }

            for (int i = 0; i < scratched.Length; i++)
            {
                if (Vector3.Distance(scratched[i].velocity, Vector3.zero) < Vector3.kEpsilon)
                {
                    iterado++;
                }
            }

            if (Vector3.Distance(white.velocity, Vector3.zero) < Vector3.kEpsilon)
            {
                iterado++;
            }

            if (Vector3.Distance(black.velocity, Vector3.zero) < Vector3.kEpsilon)
            {
                iterado++;
            }

            if (iterado == scratched.Length + smooth.Length + 2)
            {
                endTurn?.Invoke();
                playerTurn = !playerTurn;

                player1.SetActive(playerTurn);
                player2.SetActive(!playerTurn);
                wait = false;
                Debug.Log("switch turn");
            }
        }
    }

    private void CheckWinCondition()
    {
        if(playerTurn)
        {
            if(smoothMatchPoint)
            {
                Debug.Log("Player 1 win");
                player1win.SetActive(true);
            }
            else
            {
                Debug.Log("Player 2 win");
                player2win.SetActive(true);
            }
        }
        else
        {
            if (scratchedMatchPoint)
            {
                Debug.Log("Player 2 win");
                player2win.SetActive(true);
            }
            else
            {
                Debug.Log("Player 1 win");
                player1win.SetActive(true);
            }
        }
    }

}
