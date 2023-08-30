using UnityEngine;
using UnityEngine.UI;


namespace Mkey
{
    public class LevelButton : MonoBehaviour
    {
        public GameObject LeftStar;
        public GameObject MiddleStar;
        public GameObject RightStar;
        public GameObject Lock;
        public Button button;
        public GameObject LockText;
        public Text numberText;
        public bool Interactable { get; private set; }

        internal void SetActive(bool active, int activeStarsCount, bool isPassed)
        {
            if (LeftStar) LeftStar.SetActive(activeStarsCount > 0);// && isPassed);
            if (MiddleStar) MiddleStar.SetActive(activeStarsCount > 1);// && isPassed);
            if (RightStar) RightStar.SetActive(activeStarsCount > 2);// && isPassed);
            Interactable = active;// || isPassed;
            if(button)  button.interactable = Interactable;

            if (isPassed)
            {
                Color color = button.GetComponent<Image>().color;
                if (color != null)
                {
                    Debug.Log(color.a);
                    color.a = 0.55f;
                    button.GetComponent<Image>().color = color;
                }

            }

            if (active)
                MapController.Instance.ActiveButton = this;

            if (Lock)
            {
                Lock.SetActive(!isPassed && !active);
                
            }
            
            if(LockText) LockText.SetActive(active || isPassed);
            numberText.gameObject.SetActive(active || isPassed);
        }
    }
}