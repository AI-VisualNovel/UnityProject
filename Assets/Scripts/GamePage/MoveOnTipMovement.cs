using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnTipMovement : MonoBehaviour
{
    public float moveRange = 2.0f;  // 上下移動的範圍
    public float moveSpeed = 1.0f;  // 移動速度

    private Vector3 startPos;  // 起始位置

    private void Start()
    {
        startPos = transform.position;  // 儲存起始位置
    }

    private void Update()
    {
        // 計算上下移動的目標位置
        Vector3 targetPosition = startPos + Vector3.up * Mathf.Sin(Time.time * moveSpeed) * moveRange;

        // 更新物件的位置
        transform.position = targetPosition;
    }
}
