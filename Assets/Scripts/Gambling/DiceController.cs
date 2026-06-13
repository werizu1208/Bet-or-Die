using UnityEngine;
using System;
using System.Collections;

public class DiceController : MonoBehaviour
{
    [SerializeField] private Rigidbody[] dice;
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private float throwTorque = 3f;
    [SerializeField] private float settleVelocityThreshold = 0.05f;
    [SerializeField] private float settleCheckInterval = 0.3f;

    public event Action<int[]> OnDiceSettled;

    public void ThrowDice()
    {
        foreach (var die in dice)
        {
            die.linearVelocity = Vector3.zero;
            die.angularVelocity = Vector3.zero;
            die.AddForce(new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(0.5f, 1f),
                UnityEngine.Random.Range(-1f, 1f)
            ) * throwForce, ForceMode.Impulse);
            die.AddTorque(UnityEngine.Random.insideUnitSphere * throwTorque, ForceMode.Impulse);
        }
        StartCoroutine(WaitForSettle());
    }

    private IEnumerator WaitForSettle()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            yield return new WaitForSeconds(settleCheckInterval);
            bool allSettled = true;
            foreach (var die in dice)
            {
                if (die.linearVelocity.magnitude > settleVelocityThreshold ||
                    die.angularVelocity.magnitude > settleVelocityThreshold)
                {
                    allSettled = false;
                    break;
                }
            }
            if (allSettled) break;
        }

        int[] results = new int[dice.Length];
        for (int i = 0; i < dice.Length; i++)
            results[i] = ReadFaceValue(dice[i].transform);

        OnDiceSettled?.Invoke(results);
    }

    // サイコロのローカルY軸が上向きの面を読む
    private int ReadFaceValue(Transform dieTransform)
    {
        Vector3[] faceNormals = {
            Vector3.up,    // 1
            Vector3.down,  // 6
            Vector3.right, // 2
            Vector3.left,  // 5
            Vector3.forward, // 3
            Vector3.back   // 4
        };
        int[] faceValues = { 1, 6, 2, 5, 3, 4 };

        float maxDot = -1f;
        int result = 1;
        for (int i = 0; i < faceNormals.Length; i++)
        {
            float dot = Vector3.Dot(dieTransform.TransformDirection(faceNormals[i]), Vector3.up);
            if (dot > maxDot) { maxDot = dot; result = faceValues[i]; }
        }
        return result;
    }
}
