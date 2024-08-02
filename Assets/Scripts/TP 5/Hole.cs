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

    private void OnEnable()
    {
        gameManager.endTurn += SpawnWhite;
    }

    private void OnDestroy()
    {
        gameManager.endTurn -= SpawnWhite;
    }

    void Update()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            if (balls[i].isActive && Vector3.Distance(transform.position, balls[i].position) < actionRadius)
            {
                if (balls[i].typeBall == TypeBall.White)
                {
                    balls[i].gameObject.transform.position = new Vector3(0, 100, 0);
                    balls[i].velocity = Vector3.zero;
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

    void SpawnWhite()
    {
        if(!white.isActive)
        {
            white.isActive = true;
            white.gameObject.transform.position = spawnBall.position;
            Debug.Log($"Return : {white.gameObject.name}");
            white.velocity = Vector3.zero;
        }
    }
}
