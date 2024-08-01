using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private Ball[] balls;
    [SerializeField] private Transform spawnBall;
    [SerializeField] private float actionRadius;

    void Update()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            if(Vector3.Distance(transform.position, balls[i].position) < actionRadius)
            {
                if (balls[i].gameObject.tag == "White")
                {

                    balls[i].gameObject.transform.position = spawnBall.position;
                    Debug.Log($"Return : { balls[i].gameObject.name}");
                    balls[i].velocity = Vector3.zero;
                }
                else
                {
                    Debug.Log($"Drop : { balls[i].gameObject.name}");
                    balls[i].gameObject.transform.position = new Vector3(0, -100, 0);
                    balls[i].gameObject.SetActive(false);
                }

            }
        }
    }
}
