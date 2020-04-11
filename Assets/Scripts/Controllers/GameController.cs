using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        public GameObject eventSystem;
        public BlackHoleController blackHoleController;

        private int _levelCount;
        private int _stageCount;
        
        private bool _startGame;
        private Stage _currentStage;

        private const float START_CAM_Z = -3.69f;

        void Start()
        {
            InitProperties();
            SetupCamera();
        }

        void SetupCamera()
        {
            if((float)Camera.main.pixelHeight > Camera.main.pixelWidth)
                Camera.main.fieldOfView = 35 * (1920 / 1080) / ((float)Camera.main.pixelWidth / Camera.main.pixelHeight);
            else
                Camera.main.fieldOfView = 35 * (1920 / 1080) / ((float)Camera.main.pixelHeight / Camera.main.pixelWidth);
        }

        private void InitProperties()
        {
            _stageCount = 0;
            _startGame = false;
            InitEvents();
            StartGame();
        }

        private void NextGenerateStage(System.Object arg = null)
        {
            _startGame = false;
            _stageCount++;
            blackHoleController.transform.position = new Vector3(0, blackHoleController.transform.position.y, 0);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, START_CAM_Z);
            _currentStage = Resources.Load<Stage>("ScriptableObject/Stages/Stage_" + _stageCount);
            if(_currentStage == null)
            {
                Managers.EventManager.TriggerEvent(Enums.Action.SendMessage.ToString(), Enums.Information.LevelsFinished);
            }
            else
            {
                Managers.EventManager.TriggerEvent(Enums.Action.CreateStage.ToString(), _currentStage);
            }
        }

        private void InitEvents()
        {
            Managers.EventManager.StartListening(Enums.Action.TurnMain.ToString(), TurnMain);
            Managers.EventManager.StartListening(Enums.Action.NextGenerateLevel.ToString(), ChangeNextLevel);
            Managers.EventManager.StartListening(Enums.Action.NextGenerateStage.ToString(), NextGenerateStage);
            Managers.EventManager.StartListening(Enums.Action.RestartGame.ToString(), RestartGame);
            Managers.EventManager.StartListening(Enums.Action.GameOver.ToString(), GameOver);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_startGame)
                {
                    Managers.EventManager.TriggerEvent(Enums.Action.StartGame.ToString(),_levelCount);
                    _startGame = true;
                }
            }
        }

        void StartGame(System.Object arg = null)
        {
            NextGenerateStage();
        }

        void GameOver(System.Object success = null)
        {
            if ((bool)success)
            {
                blackHoleController.NextPlatformAnimation();
                BlockElements(true);
            }
            else
            {
                BlockGame(true);
            }
        }

        void TurnMain(System.Object arg = null)
        {
            _levelCount = 0;
            _stageCount = 0;
            StartGame();
        }

        void BlockGame(bool value)
        {
            BlockUI(value);
            BlockElements(value);
        }

        void BlockUI(bool block)
        {
            eventSystem.SetActive(!block);
        }

        void BlockElements(bool block)
        {
            //blackHoleController.GetComponent<BlackHoleController>().enabled = !block;
        }

        void RestartGame(System.Object arg = null)
        {
            _startGame = false;
            blackHoleController.transform.position = new Vector3(0, blackHoleController.transform.position.y, 0);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, START_CAM_Z);
            _currentStage = Resources.Load<Stage>("ScriptableObject/Stages/Stage_" + _stageCount);
            Managers.EventManager.TriggerEvent(Enums.Action.CreateStage.ToString(), _currentStage);
        }

        private void MoveStart(System.Object arg = null)
        {
            BlockGame(true);
        }

        void ChangeNextLevel(System.Object arg = null)
        {
            BlockGame(false);
        }
    }
}
