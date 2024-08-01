using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Inputs : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Menu menu;
    [SerializeField] private Transform whiteSpawn;

    Vector3 start;
    Vector3 end;

    bool inMenu = true;
    bool startIsReady = false;

    private void OnEnable()
    {
        menu.play += OnPlay;
    }

    private void OnDestroy()
    {
        menu.play -= OnPlay;
    }

    void Update()
    {
        if(!inMenu)
        {
            if (Input.GetMouseButtonDown(0) && !gameManager.wait)
            {
                start = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                startIsReady = true;
            }

            if (Input.GetMouseButtonUp(0) && !gameManager.wait && startIsReady)
            {
                end = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

                gameManager.wait = true;
                startIsReady = false;

                ball.ApplyForce((start - end) * forceMultiplier);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                ball.gameObject.transform.position = whiteSpawn.position;
                ball.velocity = Vector3.zero;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Pool");
        }
    }


    private void OnPlay()
    {
        inMenu = false;
        ball.isActive = true;
    }

    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
            return;

        Gizmos.color = Color.yellow;

        if(startIsReady)
        {
            Vector3 preEnd = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            Vector3 dir = preEnd - start;

            Gizmos.DrawLine(ball.position, ball.position - dir);
        }
    }
}
