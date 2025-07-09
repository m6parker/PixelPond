using System;
using System.Collections;
using Unity.Cinemachine;
// using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapTransition : MonoBehaviour
{

    [SerializeField] PolygonCollider2D mapBoundary;
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;
    [SerializeField] float additivePos = 2f;
    public CinemachineCamera virtualCamera;
    private enum Direction { Up, Down, Left, Right }


    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();

        // Debug.Log("awake " + confiner.BoundingShape2D);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Switched to: " + mapBoundary.name);
        virtualCamera.PreviousStateIsValid = false;

        // Debug.Log(virtualCam.Follow?.name ?? "Follow is null!");
        if (!collision.CompareTag("Player")) return;

        // Set the new bounding shape
        confiner.BoundingShape2D = mapBoundary;

        // Invalidate cache AFTER assigning the new collider
        confiner.InvalidateBoundingShapeCache();

        // Move the player
        StartCoroutine(MovePlayerNextFrame(collision.gameObject));

        MapController_manual.Instance?.HighlightArea(mapBoundary.name);
    }


    private IEnumerator MovePlayerNextFrame(GameObject player)
    {
        yield return new WaitForEndOfFrame(); // Give Cinemachine a frame to catch up
        UpdatePlayerPosition(player);
    }
    

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;
        // Debug.Log("going " + direction);
        

        switch (direction)
        {
            case Direction.Up:
                newPos.y += additivePos;
                break;
            case Direction.Down:
                newPos.y -= additivePos;
                break;
            case Direction.Left:
                newPos.x -= additivePos;
                break;
            case Direction.Right:
                newPos.x += additivePos;
                break;
        }

        player.transform.position = newPos;
    }

}
