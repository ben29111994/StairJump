using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public RectTransform[] topUI;
    public LayerMask layerCloud;
    void Start()
    {
        SetFOV();
    }

    void SetFOV()
    {
        float ratio = Camera.main.aspect;

        if (ratio >= 0.74) // 3:4
        {
            Camera.main.fieldOfView = 60;
        }
        else if (ratio >= 0.56) // 9:16
        {
            Camera.main.fieldOfView = 60;
        }
        else if (ratio >= 0.45) // 9:19
        {
            Camera.main.fieldOfView = 65;

            foreach (RectTransform r in topUI)
            {
                Vector2 current = r.anchoredPosition;
                current.y -= 150.0f;
                r.anchoredPosition = current;
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateRaycastFade();
    }

    public void UpdateRaycastFade()
    {
        Transform _player = GameManager.Instance.RunDunk.players[0].rigid.transform;
        Vector3 origin = transform.position;
        Vector3 direction = _player.position - origin;
        float distance = direction.magnitude;
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 1.0f , out hit, 100.0f,layerCloud))
        {
            Cloud _cloud = hit.collider.gameObject.GetComponent<Cloud>();
            if(_cloud != null)
            {
                _cloud.Fade();
            }
        }
    }
}