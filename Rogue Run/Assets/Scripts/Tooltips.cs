using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltips : MonoBehaviour
{
    //tooltip of each upgrade
    [SerializeField] private GameObject cdTip;
    [SerializeField] private GameObject backTip;
    [SerializeField] private GameObject firstTip;
    [SerializeField] private GameObject roomTip;
    [SerializeField] private GameObject darkTip;
    [SerializeField] private GameObject killTip;

    //trigger of each tooltip
    [SerializeField] private GameObject cd_trigger;
    [SerializeField] private GameObject back_trigger;
    [SerializeField] private GameObject first_trigger;
    [SerializeField] private GameObject room_trigger;
    [SerializeField] private GameObject dark_trigger;
    [SerializeField] private GameObject kill_trigger;


    private void Update()
    {
        //gets pointer
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        
        //raycasts 
        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

        //if not on anything
        if (raycastResultList.Count == 0)
        {
            cdTip.SetActive(false);
            backTip.SetActive(false);
            firstTip.SetActive(false);
            roomTip.SetActive(false);
            darkTip.SetActive(false);
            killTip.SetActive(false);
        }

        //otherwise show the tip of the respective upgrade
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject == cd_trigger)
            {
                cdTip.SetActive(true);
            }
            else if (raycastResultList[i].gameObject == back_trigger)
            {
                backTip.SetActive(true);
            }
            else if (raycastResultList[i].gameObject == first_trigger)
            {
                firstTip.SetActive(true);
            }
            else if (raycastResultList[i].gameObject == room_trigger)
            {
                roomTip.SetActive(true);
            }
            else if (raycastResultList[i].gameObject == dark_trigger)
            {
               darkTip.SetActive(true);
            }
            else if (raycastResultList[i].gameObject == kill_trigger)
            {
                killTip.SetActive(true);
            }
        }
    }
}