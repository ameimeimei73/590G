using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = new Vector3(player.forward.x, 0, player.forward.z).normalized;
        transform.position = player.position + new Vector3(forward.x * offset.z, offset.y, offset.z * forward.z);
        transform.LookAt(player);
    }
}
