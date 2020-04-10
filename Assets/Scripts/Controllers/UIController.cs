using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Controller
{
    public class UIController : MonoBehaviour
    {
        [Header("Hole MoveSpeed")]
        public Button turnMainButton;
        [Header("Okay Button")]
        public Button okButton;
        [Header("Next Button")]
        public Button nextButton;
        [Header("Restart Button")]
        public Button restartButton;
        [Header("Information Text about game general")]
        public Text informationText;
        [Header("Info Popup Container")]
        public GameObject infoPopupContainer;
        [Header("HandInfo Text")]
        public Text handInfoText;
        [Header("HandInfo Image")]
        public Image handInfoImage;
        [Header("Level1 SliderBar/progress bar")]
        public Slider progresBar_1;
        [Header("Level2 SliderBar/progress bar")]
        public Slider progresBar_2;
        [Header("Level1 level information text")]
        public Text levelText_1;
        [Header("Level2 level information text")]
        public Text levelText_2;

        void Awake()
        {
            InitProperties();
        }

        private void InitProperties()
        {
            InitEvents();
            restartButton.gameObject.SetActive(false);
            turnMainButton.gameObject.SetActive(false);
            infoPopupContainer.SetActive(false);
            handInfoText.gameObject.SetActive(true);
            handInfoImage.gameObject.SetActive(true);
        }

        private void InitEvents()
        {
            Managers.EventManager.StartListening(Enums.Action.UpdateLevelProgress.ToString(), UpdateLevelProgress);
            Managers.EventManager.StartListening(Enums.Action.StartGame.ToString(), StartGame);
            Managers.EventManager.StartListening(Enums.Action.SendMessage.ToString(), SendMessage);
            okButton.onClick.AddListener(() => PopupContainerVisible(false));
            nextButton.onClick.AddListener(() => NextStage());
            turnMainButton.onClick.AddListener(() => TurnMain());
            restartButton.onClick.AddListener(() => RestartGame());
        }

        void PopupContainerVisible(bool active)
        {
            infoPopupContainer.SetActive(active);
        }

        void NextStage()
        {
            PopupContainerVisible(false);
            Managers.EventManager.TriggerEvent(Enums.Action.NextGenerateStage.ToString());
        }

        void SendMessage(System.Object message = null)
        {
            Enums.Information info = (Enums.Information)message;
            string m = "";
            switch (info)
            {
                case Enums.Information.LevelsFinished:
                    okButton.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(false);
                    turnMainButton.gameObject.SetActive(true);
                    restartButton.gameObject.SetActive(false);
                    m = "Başka bölüm yok bitti";
                    break;
                case Enums.Information.LevelSuccess:
                    okButton.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);
                    turnMainButton.gameObject.SetActive(false);
                    restartButton.gameObject.SetActive(false);
                    m = "Bir sonraki bölüme geçtin";
                    break;
                case Enums.Information.Failed:
                    okButton.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(false);
                    turnMainButton.gameObject.SetActive(false);
                    restartButton.gameObject.SetActive(true);
                    m = "Yandın.Bölümü Tekrarla";
                    break;
            }
            informationText.text = m;
            PopupContainerVisible(true);
            //belki buraya animasyon vs eklenir.
        }

        void TurnMain()
        {
            turnMainButton.gameObject.SetActive(false);
            handInfoImage.gameObject.SetActive(true);
            handInfoText.gameObject.SetActive(true);
            PopupContainerVisible(false);
            Managers.EventManager.TriggerEvent(Enums.Action.TurnMain.ToString());
        }

        void StartGame(System.Object levelCount)
        {
            handInfoText.gameObject.SetActive(false);
            handInfoImage.gameObject.SetActive(false);
        }

        /// <summary>  
        ///   Vector2 pO = new Vector2(p, _levelCount);
        /// </summary>
        /// <param name="arg"></param>
        void UpdateLevelProgress(System.Object arg)
        {
            Vector2 pO = (Vector2)arg;
            if (pO.y % 2 == 1)
            {
                progresBar_1.DOValue(pO.x, .2f);
                levelText_1.text = pO.y.ToString();
            }
            if (pO.y % 2 == 0)
            {
                levelText_2.text = pO.y.ToString();
                progresBar_2.DOValue(pO.x, .2f);
            }
        }

        void RestartGame()
        {
            handInfoImage.gameObject.SetActive(true);
            handInfoText.gameObject.SetActive(true);
            PopupContainerVisible(false);
            restartButton.gameObject.SetActive(false);
            Managers.EventManager.TriggerEvent(Enums.Action.RestartGame.ToString());
        }
    }
}
