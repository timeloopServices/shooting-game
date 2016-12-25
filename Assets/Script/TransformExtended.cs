using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class TransformExtended
{
    static List<TransformOperation> operations = new List<TransformOperation>();

    public static void DoAnimate(this Transform trans, Transform target, float time)
    {
        TransformOperation to = GetTransformOperation(trans);
        to.StartAnimation(target, time);
    }

    public static void DoJump(this Transform trans, Transform target, float jumpRate, float time)
    {
        TransformOperation to = GetTransformOperation(trans);
        to.StartJump(target, jumpRate, time);
    }

    static TransformOperation GetTransformOperation(Transform t)
    {
        foreach (TransformOperation to in operations)
        {
            if (to.current.Equals(t))
            {
                return to;
            }
        }
        GameObject obj = new GameObject();
        TransformOperation operation = obj.AddComponent<TransformOperation>();
        operation.current = t;
        operations.Add(operation);
        return operation;
    }
}

public class TransformOperation : MonoBehaviour
{
    public Transform current;
    public void StartAnimation(Transform target, float time)
    {
        StartCoroutine(Movement(target, time));
    }

    private IEnumerator Movement(Transform target, float time)
    {
        float index = 0f;
        float rate = 1.0f / time;
        Vector3 startPos = current.position;
        do
        {
            current.position = Vector3.Lerp(startPos, target.position, index);
            index += Time.fixedDeltaTime * rate;
            yield return new WaitForEndOfFrame();
        } while (index <= 1f);
        current.position = target.position;
    }

    public void StartJump(Transform target, float jumpRate, float time)
    {
        StartCoroutine(Jump(target, jumpRate, time));
    }

    private IEnumerator Jump(Transform target, float jumpRate, float time)
    {
        float index = 0f;
        float rate = 1.0f / time;
        AnimationCurve animCurve = new AnimationCurve();
        animCurve.AddKey(0f, 0f);
        animCurve.AddKey(0.5f, 1f);
        animCurve.AddKey(1f, 0f);
        Vector3 startPos = current.position;
        do
        {
            Vector3 pos = Vector3.Lerp(startPos, target.position, index);
            pos.y += animCurve.Evaluate(index) * jumpRate;
            transform.position = pos;
            index += Time.fixedDeltaTime * rate;
            yield return new WaitForEndOfFrame();
        } while (index <= 1f);
        current.position = target.position;
    }

}

