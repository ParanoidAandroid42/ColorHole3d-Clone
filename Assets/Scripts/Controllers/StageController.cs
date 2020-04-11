
using UnityEngine;
using DG.Tweening;

public class StageController : MonoBehaviour
{
    [Header("platform base MeshRenderer/1")]
    public MeshRenderer platformBase_01;
    [Header("platform's corner MeshRenderer/1")]
    public MeshRenderer platformBlock_01;
    [Header("platform base MeshRenderer/2")]
    public MeshRenderer platformBase_02;
    [Header("platform's corner MeshRenderer/2")]
    public MeshRenderer platformBlock_02;
    [Header("door MeshRenderer/2")]
    public MeshRenderer door;
    [Header("tube MeshRenderer")]
    public MeshRenderer tube;
    public BlackHoleController blackHoleController;

    private int _levelCount;
    private int _boxesCount;
    private Stage _stage;
    private Level _currentLevel;
    private GameObject _boxesParent;

    private Vector2 _doorStarPosition;

    public void Start()
    {
        InitEvents();
        _doorStarPosition = door.transform.position;
    }

    public void InitEvents()
    {
        Managers.EventManager.StartListening(Enums.Action.CreateStage.ToString(), Create);
        Managers.EventManager.StartListening(Enums.Action.NextGenerateLevel.ToString(), NextCreateLevel);
        Managers.EventManager.StartListening(Enums.Action.CheckBoxes.ToString(), CheckBoxes);
    }

    public void Create(System.Object arg=null)
    {
        _levelCount = 0;
        _stage = (Stage)arg;
        PreviousDestroyElements();
        platformBase_01.material = _stage.levels[0].platform;
        platformBase_02.material = _stage.levels[1].platform;
        platformBlock_01.material = _stage.levels[0].platformBlock;
        platformBlock_02.material = _stage.levels[1].platformBlock;
        door.material = _stage.doorMetarial;
        tube.material = _stage.tubeMetarial;
        NextCreateLevel();
        Vector2 pO = new Vector2(0, _stage.levels[0].levelId);
        Managers.EventManager.TriggerEvent(Enums.Action.UpdateLevelProgress.ToString(), pO);
        
        pO = new Vector2(0, _stage.levels[1].levelId);
        Managers.EventManager.TriggerEvent(Enums.Action.UpdateLevelProgress.ToString(), pO);
        door.gameObject.transform.DOMoveY(_doorStarPosition.y + 2, 1);
        blackHoleController.UpdatePlatformSettings(platformBase_01.gameObject);
    }

    private void CheckBoxes(System.Object arg = null)
    {
        _boxesCount--;
        if(_boxesCount <= 0)
        {
            PreviousDestroyElements();
            if (_levelCount < 2)
            {
                NextPlatformAnimation();
            }
            else
            {
                //confeti animasyonu eklenebilir todo
                door.gameObject.transform.DOMoveY(_doorStarPosition.y+2, 1);
                Managers.EventManager.TriggerEvent(Enums.Action.SendMessage.ToString(),Enums.Information.LevelSuccess);
            }
        }
        float p = (float)(_currentLevel.boxTotalCount - _boxesCount) / (float)_currentLevel.boxTotalCount;
        Vector2 pO = new Vector2(p, _currentLevel.levelId);
        Managers.EventManager.TriggerEvent(Enums.Action.UpdateLevelProgress.ToString(), pO);
    }

    public void NextCreateLevel(System.Object arg = null)
    {
        _levelCount++;
        _currentLevel = Resources.Load<Level>("ScriptableObject/Levels/Level_" + _stage.levels[_levelCount-1].levelId);
        _boxesCount = _currentLevel.boxTotalCount;
        GameObject boxes = Instantiate<GameObject>(_currentLevel.prefab);
        _boxesParent = boxes;
        blackHoleController.UpdatePlatformSettings(platformBase_02.gameObject);
    }

    public void PreviousDestroyElements()
    {
        if(_boxesParent!=null)
        Destroy(_boxesParent);
    }

    public void NextPlatformAnimation()
    {
        door.gameObject.transform.DOMoveY(door.gameObject.transform.position.y-2, 1);
        Managers.EventManager.TriggerEvent(Enums.Action.GameOver.ToString(), true);
    }
}
