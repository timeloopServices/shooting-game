using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            StartCoroutine(ChangePipe());
        }
    }

    private IEnumerator ChangePipe()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        PipeManager.instance.ChangePipe();
    }
}
