using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetValueTipControllerFantasy : MonoBehaviour
{
    public float moveDistance = 1.0f;
    public float moveSpeed = 2.0f;
    public float pauseDuration = 3.0f;
    public float returnSpeed = 3.0f;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition - new Vector3(0, moveDistance, 0);
        //StartCoroutine(MoveObject());
    }

    public IEnumerator MoveObject()
    {
        // 移動到指定位置
        float journeyLength = Vector3.Distance(startPosition, endPosition);
        float startTime = Time.time;
        float distanceCovered = 0.0f;

        while (distanceCovered < journeyLength)
        {
            float journeyTime = (Time.time - startTime) * moveSpeed;
            distanceCovered = Mathf.Lerp(0, journeyLength, journeyTime);
            transform.position = Vector3.Lerp(startPosition, endPosition, distanceCovered / journeyLength);
            yield return null;
        }

        // 停留一段時間
        yield return new WaitForSeconds(pauseDuration);

        // 返回原位置
        startTime = Time.time;
        distanceCovered = 0.0f;

        while (distanceCovered < journeyLength)
        {
            float journeyTime = (Time.time - startTime) * returnSpeed;
            distanceCovered = Mathf.Lerp(0, journeyLength, journeyTime);
            transform.position = Vector3.Lerp(endPosition, startPosition, distanceCovered / journeyLength);
            yield return null;
        }
    }
}

