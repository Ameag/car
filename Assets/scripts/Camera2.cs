using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{

    [SerializeField] private Transform Player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    Quaternion startRotation;
    Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {
        offset = transform.position-Player.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Player.position + Player.rotation * offset, moveSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Player.rotation * startRotation,rotationSpeed * Time.fixedDeltaTime);

    }
}
