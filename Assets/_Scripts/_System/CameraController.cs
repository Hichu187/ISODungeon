using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera m_Camera;
    public GameObject player;
    void Start()
    {
        m_Camera = GetComponent<Camera>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPosition = m_Camera.WorldToViewportPoint(player.transform.position);


    }

    public void POVCheck(Vector3 viewportPosition)
    {
        if (viewportPosition.x >= 0.25f && viewportPosition.x <= 0.75f &&viewportPosition.y >= 0.25f && viewportPosition.y <= 0.75f && viewportPosition.z >= 0.25f)
        {
            Debug.Log("inside");
        }
        else
        {
            Debug.Log("outside");

            transform.DOMove(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), 1f);
        }
    }
}
