using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeManager : MonoBehaviour
{
    public static PipeManager instance;
    public GameObject pipePrefab;
    public GameObject ballPrefab;
    public float radius = 5f;
    public int totalNumber = 5;
    public float totalAngle = 90f;
    public float startAngle = 30f;
    GameObject currentActivePipe;
    List<GameObject> pipes;
    GameObject ball;

    void Awake()
    {
        instance = this;
        pipes = new List<GameObject>();
    }

    IEnumerator Start()
    {
        float thetaDifference = totalAngle / (totalNumber - 1);
        float theta = startAngle;
        for (int i = 0; i < totalNumber; i++)
        {
            float x = radius * Mathf.Cos(theta * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(theta * Mathf.Deg2Rad);
            GameObject pipe = Instantiate(pipePrefab, new Vector3(x, 0f, y), Quaternion.identity) as GameObject;
            pipe.GetComponent<Renderer>().material.color = Random.ColorHSV();
            theta += thetaDifference;
            pipes.Add(pipe);
            yield return new WaitForSeconds(0.1f);
        }

        StartPlaying();
    }

    void StartPlaying()
    {
        ball = Instantiate(ballPrefab) as GameObject;
        ChangePipe();
    }

    public void ChangePipe()
    {
        ball.GetComponent<Rigidbody>().isKinematic = true;
        currentActivePipe = pipes[Random.Range(0, pipes.Count - 1)];
        ball.transform.position = currentActivePipe.transform.position;
        StartCoroutine(PrepareForThrow(currentActivePipe));
    }

    IEnumerator PrepareForThrow(GameObject pipe)
    {
        ball.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(0.5f);

        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(500f,800f));

        yield return new WaitForSeconds(1f);
        ball.GetComponent<SphereCollider>().enabled = true;
    }
}
