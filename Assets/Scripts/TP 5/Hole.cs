using System;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Ball[] balls;
    [SerializeField] private Ball white;
    [SerializeField] private Transform spawnBall;
    [SerializeField] private float actionRadius;

    public Action blackInHole;

    private bool waitEndTurn;

    private void OnEnable()
    {
        gameManager.endTurn += SetEndTurn;
    }

    private void OnDestroy()
    {
        gameManager.endTurn -= SetEndTurn;
    }

    void Update()
    {
        if (waitEndTurn)
        {
            if(!white.isActive)
            {
                white.isActive = true;
                white.gameObject.transform.position = spawnBall.position;
                Debug.Log($"Return : {white.gameObject.name}");
                white.velocity = Vector3.zero;
                waitEndTurn = false;
            }
        }


        for (int i = 0; i < balls.Length; i++)
        {
            if (balls[i].isActive && Vector3.Distance(transform.position, balls[i].position) < actionRadius)
            {
                if (balls[i].typeBall == TypeBall.White)
                {
                    balls[i].gameObject.transform.position = new Vector3(0, 100, 0);
                    balls[i].isActive = false;
                }
                else if (balls[i].typeBall == TypeBall.Black)
                {
                    blackInHole?.Invoke();
                    Debug.Log($"Drop : {balls[i].gameObject.name}");
                    balls[i].isActive = false;
                    balls[i].velocity = Vector3.zero;
                    balls[i].gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log($"Drop : {balls[i].gameObject.name}");
                    balls[i].isActive = false;
                    balls[i].velocity = Vector3.zero;
                    balls[i].gameObject.SetActive(false);
                }

            }
        }
    }

    private void SetEndTurn()
    {
        waitEndTurn = true;
    }
}
