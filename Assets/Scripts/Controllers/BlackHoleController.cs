using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

public class BlackHoleController : MonoBehaviour
{
    const int LAYER_ON_ENTER = 9;
    
    private Vector3 _offset;
    private float _mzCord;

    private const float NEXT_LEVEL_BLACKHOLE_Z = 22.14f;
    private const float CAMERA_POSITION_X = 0;
    private const float CAMERA_POSITION_Y = 10.83f;
    private const float CAMERA_POSITION_Z = -3.69f;

    private bool draggable = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Enums.Tag.box.ToString() || other.tag == Enums.Tag.forbidden.ToString())
            other.gameObject.layer = LAYER_ON_ENTER;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == Enums.Tag.box.ToString())
        {
            Destroy(other.gameObject);
            Managers.EventManager.TriggerEvent(Enums.Action.CheckBoxes.ToString());
        }
        if (other.tag == Enums.Tag.forbidden.ToString())
        {
            Camera.main.transform.DOShakePosition(1).onComplete = () =>
            {
                Managers.EventManager.TriggerEvent(Enums.Action.SendMessage.ToString(), Enums.Information.Failed);
                Camera.main.transform.position = new Vector3(CAMERA_POSITION_X,CAMERA_POSITION_Y,CAMERA_POSITION_Z);
            };
        }
    }

    public void NextPlatformAnimation()
    {
        transform.DOMoveX(0, 1).onComplete = () =>
        {
            transform.DOMoveZ(NEXT_LEVEL_BLACKHOLE_Z, 5).onComplete = () =>
            {
                Managers.EventManager.TriggerEvent(Enums.Action.NextGenerateLevel.ToString());
            };
            Camera.main.transform.DOMoveZ(Camera.main.transform.position.z + 22, 5);
        };
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _offset = gameObject.transform.position - GetMouseWorldPos();
        }

        if (Input.GetMouseButton(0))
        {
            transform.position = GetMouseWorldPos() + _offset;
            transform.position = new Vector3(transform.position.x, -0.35f, transform.position.z);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        _mzCord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        mousePoint.z = _mzCord;
        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    private void Update()
    {
        Move();
    }
}
