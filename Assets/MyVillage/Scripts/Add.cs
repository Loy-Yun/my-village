﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Specialized;
using System.Security.Cryptography;

public class Add : MonoBehaviour
{
    [SerializeField] private GameObject m_AddWindow = null;
    [SerializeField] private Button m_AddButton = null;
    [SerializeField] private Button m_BackButton = null;

    [SerializeField] private Button[] m_TabButtonList = new Button[7];
    [SerializeField] private Button[] m_HouseList = new Button[2];
    [SerializeField] private Button[] m_TreeList = new Button[2];
    [SerializeField] private Button[] m_FarmList = new Button[3];
    [SerializeField] private GameObject[] m_TabList = new GameObject[7];

    [SerializeField] private GameObject m_FixAlertPanel = null;
    [SerializeField] private Button m_FixButton = null;
    [SerializeField] private Button m_RotateButton = null;
    [SerializeField] private Button m_FixBackButton = null;

    [SerializeField] private GameObject[] HouseObj = new GameObject[2];
    [SerializeField] private GameObject[] TreeObj = new GameObject[2];
    [SerializeField] private GameObject[] FarmObj = new GameObject[3];

    private Touch touch;
    private GameObject addObj;
    private bool add = false;
    private Quaternion rotationY;
    private float speedModifier = 0.001f;


    // Start is called before the first frame update
    void Start()
    {
        m_AddButton.onClick.AddListener(_OnAddButtonClicked);
        m_BackButton.onClick.AddListener(_OnBackButtonClicked);

        for (int i=0; i<m_TabButtonList.Length; i++) {
            int buttonIndex = i;
            m_TabButtonList[buttonIndex].onClick.AddListener(() => _OnTabButtonClicked(buttonIndex));
        }

        for (int i=0; i<m_HouseList.Length; i++) {
            int buttonIndex = i;
            m_HouseList[buttonIndex].onClick.AddListener(() => _OnHouseButtonClicked(buttonIndex));
        }

        for (int i=0; i<m_TreeList.Length; i++) {
            int buttonIndex = i;
            m_TreeList[buttonIndex].onClick.AddListener(() => _OnTreeButtonClicked(buttonIndex));
        }

        for (int i=0; i<m_FarmList.Length; i++) {
            int buttonIndex = i;
            m_FarmList[buttonIndex].onClick.AddListener(() => _OnFarmButtonClicked(buttonIndex));
        }

        m_FixButton.onClick.AddListener(_OnFixButtonClicked);
        m_RotateButton.onClick.AddListener(_OnRotateButtonClicked);
        m_FixBackButton.onClick.AddListener(_OnFixBackButtonClicked);
    }

    void Update()
    {
        if (add)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                // If the player has not touched the screen, we are done with this update.
                /*
                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                {
                    return;
                }

                // Should not handle input if the player is pointing on UI.
                */
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return;
                }

                // Move the object
                if (touch.phase == TouchPhase.Moved)
                {
                    addObj.transform.position = new Vector3(
                        addObj.transform.position.x + touch.deltaPosition.x * speedModifier,
                        addObj.transform.position.y,
                        addObj.transform.position.z + touch.deltaPosition.y * speedModifier);
                }
            }
        }
    }

    public void OnDestroy() {
        m_AddButton.onClick.RemoveListener(_OnAddButtonClicked);
        m_BackButton.onClick.RemoveListener(_OnBackButtonClicked);
    }

    public bool CanPlaceAsset() {
        return !m_AddWindow.activeSelf;
    }

    private void _OnAddButtonClicked() {
        m_AddWindow.SetActive(true);
    }

    private void _OnBackButtonClicked() {
        m_AddWindow.SetActive(false);
    }

    private void _OnTabButtonClicked(int idx) {

        for (int i=0; i<m_TabButtonList.Length; i++) {
            m_TabList[i].gameObject.SetActive(false);
        }
        m_TabList[idx].gameObject.SetActive(true);

    }

    private void _OnHouseButtonClicked(int idx) {
        m_AddWindow.SetActive(false);
        add = true;

        Vector3 pos = GameObject.Find("ARController").GetComponent<ARController>().Floor.transform.position;
        GameObject.Find("ARController").GetComponent<ARController>().Character.SetActive(false);
        GameObject.Find("ARController").GetComponent<ARController>().Controller.SetActive(false);

        addObj = Instantiate(HouseObj[idx], pos, Quaternion.identity);
        addObj.transform.parent = GameObject.Find("FloorPrototype64x01x64").transform;
        addObj.transform.localScale = new Vector3(3f, 3f, 3f);

        m_FixAlertPanel.gameObject.SetActive(true);
    }

    private void _OnTreeButtonClicked(int idx) {
        m_AddWindow.SetActive(false);
        add = true;

        Vector3 pos = GameObject.Find("ARController").GetComponent<ARController>().Floor.transform.position;
        GameObject.Find("ARController").GetComponent<ARController>().Character.SetActive(false);
        GameObject.Find("ARController").GetComponent<ARController>().Controller.SetActive(false);

        addObj = Instantiate(TreeObj[idx], pos, Quaternion.identity);
        addObj.transform.parent = GameObject.Find("FloorPrototype64x01x64").transform;
        addObj.transform.localScale = new Vector3(3f, 3f, 3f);

        m_FixAlertPanel.gameObject.SetActive(true);
    }

    private void _OnFarmButtonClicked(int idx) {
        m_AddWindow.SetActive(false);
        add = true;

        Vector3 pos = GameObject.Find("ARController").GetComponent<ARController>().Floor.transform.position;
        GameObject.Find("ARController").GetComponent<ARController>().Character.SetActive(false);
        GameObject.Find("ARController").GetComponent<ARController>().Controller.SetActive(false);

        addObj = Instantiate(FarmObj[idx], pos, Quaternion.identity);
        addObj.transform.parent = GameObject.Find("FloorPrototype64x01x64").transform;
        addObj.transform.localScale = new Vector3(3f, 3f, 3f);

        m_FixAlertPanel.gameObject.SetActive(true);
    }

    private void _OnFixButtonClicked() {
        m_FixAlertPanel.gameObject.SetActive(false);
        add = false;

        GameObject.Find("ARController").GetComponent<ARController>().Character.SetActive(true);
        GameObject.Find("ARController").GetComponent<ARController>().Controller.SetActive(true);
    }

    private void _OnRotateButtonClicked()
    {
        addObj.transform.Rotate(0f, -90f, 0f);
        //rotationY = Quaternion.Euler(0f, -90f, 0f);
        //-touch.deltaPosition.x * speedModifier
        //addObj.transform.rotation = rotationY * transform.rotation;
    }

    private void _OnFixBackButtonClicked()
    {
        m_FixAlertPanel.gameObject.SetActive(false);
        m_AddWindow.SetActive(true);
        add = false;
        Destroy(addObj);
        
        GameObject.Find("ARController").GetComponent<ARController>().Character.SetActive(true);
        GameObject.Find("ARController").GetComponent<ARController>().Controller.SetActive(true);
    }
}