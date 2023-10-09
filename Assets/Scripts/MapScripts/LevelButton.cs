using UnityEngine;
using UnityEngine.UI;


namespace Mkey
{
    public class LevelButton : MonoBehaviour
    {
        public GameObject LeftStar;
        public GameObject MiddleStar;
        public GameObject RightStar;
        public GameObject LeftStarEmpty;
        public GameObject MiddleStarEmpty;
        public GameObject RightStarEmpty;
        public GameObject Lock;
        public Button button;
        public Image targetImage;
        public Image LockState;
        public Image ActiveState;
        public Image PassState;
        public Image AdditionState;
        public Image AdditionStatePassed;
        public Image FinalState;
        public Image FinalStatePassed;
        public Text numberText;
        public bool Interactable { get; private set; }
        private ETypeLevel _typeLevel = ETypeLevel.simple;
        public bool IsNeedMissRebuild = false;

        internal void SetIsActive(bool active, int activeStarsCount, bool isPassed, ETypeLevel typeLevel = ETypeLevel.simple)
        {
            numberText.text = "";
            _typeLevel = typeLevel;
            //targetImage = targetImage.GetComponent<Image>();
            if (LeftStar) LeftStar.SetActive(activeStarsCount > 0);// && isPassed);
            if (MiddleStar) MiddleStar.SetActive(activeStarsCount > 1);// && isPassed);
            if (RightStar) RightStar.SetActive(activeStarsCount > 2);// && isPassed);
            Interactable = active;// || isPassed;
            if(button)  button.interactable = Interactable;
            //Debug.Log(typeLevel);
            if (isPassed)
            {
                if(typeLevel == ETypeLevel.final)
                    targetImage.sprite = FinalStatePassed.sprite;
                else if (typeLevel == ETypeLevel.additional)
                    targetImage.sprite = AdditionStatePassed.sprite;
                else 
                    targetImage.sprite = PassState.sprite;

            }
            

            if (active)
            {
                //Debug.Log("active");
                MapController.Instance.ActiveButton = this;

                if (typeLevel == ETypeLevel.final)
                {
                    targetImage.sprite = FinalState.sprite;
                    numberText.text = "";
                }
                else if (typeLevel == ETypeLevel.additional)
                {
                    targetImage.sprite = AdditionState.sprite;
                    numberText.text = "";
                }
                else if (typeLevel == ETypeLevel.mission1
                    || typeLevel == ETypeLevel.mission2)
                {
                    //imgButton.sprite = AdditionState.sprite;
                    numberText.text = "";
                }
                else
                {
                    targetImage.sprite = ActiveState.sprite;
                    numberText.color = Color.black;
                }
            }
            else
            {
                numberText.color = Color.yellow;
            }


            if (!isPassed && !active)
            {
                //Lock.SetActive(!isPassed && !active);
                
                if (typeLevel == ETypeLevel.final)
                    targetImage.sprite = FinalStatePassed.sprite;
                else if (typeLevel == ETypeLevel.additional)
                    targetImage.sprite = AdditionStatePassed.sprite;
                //else if (typeLevel == ETypeLevel.mission1
                //    || typeLevel == ETypeLevel.mission2)
                    //imgButton.sprite = AdditionStatePassed.sprite;
                else if (typeLevel == ETypeLevel.simple)
                    targetImage.sprite = LockState.sprite;

            }
            if (typeLevel == ETypeLevel.simple)
            {
                if (activeStarsCount == 0)
                    numberText.text = "0%";
                else if (activeStarsCount == 1)
                    numberText.text = "30%";
                else if (activeStarsCount == 2)
                    numberText.text = "60%";
                else
                    numberText.text = "100%";
            }
            else if(typeLevel != ETypeLevel.mission1
                    && typeLevel != ETypeLevel.mission2)
            {
                numberText.text = "";
                LeftStar.SetActive(false);
                MiddleStar.SetActive(false);
                RightStar.SetActive(false);
                LeftStarEmpty.SetActive(false);
                MiddleStarEmpty.SetActive(false);
                RightStarEmpty.SetActive(false);
            }
            else if (typeLevel == ETypeLevel.mission1
                   || typeLevel == ETypeLevel.mission2)
            {
                numberText.text = "";
            }
            //button.GetComponent<Image>().sprite = imgButton.sprite;
            //if(LockText) LockText.SetActive(active || isPassed);
            numberText.gameObject.SetActive(active || isPassed);
        }
    }
}