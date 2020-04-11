using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

public class BlackHoleController : MonoBehaviour
{
    const int LAYER_ON_ENTER = 9;
    const float NEXT_LEVEL_BLACKHOLE_Z = 22.14f;
    const float CAMERA_POSITION_X = 0;
    const float CAMERA_POSITION_Y = 10.83f;
    const float CAMERA_POSITION_Z = -3.69f;
    const float MAX_DRAG_DİSTANCE = 40f;

    public float speed = .1f;

    private float _offsetDrag;
    private Vector3 _moveDirection;
    private Vector3 _startDragPosition;

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
            Camera.main.transform.DOMoveZ(Camera.main.transform.position.z + 22.25f, 5);
        };
    }

    private void Move()
    {
        Vector3 mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            _startDragPosition = mousePos;
        }
        else if (Input.GetMouseButton(0))
        {
            _offsetDrag = (mousePos - _startDragPosition).magnitude;

            if (_offsetDrag > MAX_DRAG_DİSTANCE)
            {
                _startDragPosition = mousePos - _moveDirection * MAX_DRAG_DİSTANCE;
                _offsetDrag = (mousePos - _startDragPosition).magnitude;
            }

            _moveDirection = (mousePos - _startDragPosition).normalized;
            var direction = new Vector3(_moveDirection.x, 0, _moveDirection.y);
            transform.position += direction * speed * _offsetDrag * Time.deltaTime;
        }
    }

    private void Update()
    {
        Move();
    }
}
