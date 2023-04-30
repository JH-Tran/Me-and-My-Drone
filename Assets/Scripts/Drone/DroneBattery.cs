using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneBattery : MonoBehaviour
{
    //Disable the drone when reaching 0 battery
    [SerializeField] private DroneMovement droneMovement;
    [SerializeField] private DroneCamera droneCamera;
    //Battery fill image
    [SerializeField] private Image batteryInner;
    //Black screen
    [SerializeField] private Image blackScreen;
    //Screen Saver UI Text
    [SerializeField] private GameObject screenSaverTexts;
    [SerializeField] private Text screenOffTimer;

    private ClawGrabber clawGrabber;
    //Using battery to move drone around. 90 max time and 4 cooldown (SUBJECT TO CHANGE)
    private float batteryMaxTime = 90f;
    [SerializeField] private float batteryTime;
    //Recharing Battery
    private float batteryCooldownTime = 4f;
    private float batterRechargeTime;
    private bool isBatteryRechargeFromZero = false;
    private bool isLookingAtTablet;
    //If the drone is not disabled
    public bool isTimerOn;
    //Recharing battery from any percentage
    private float rechargeSpeed = 3;

    void Start()
    {
        droneCamera = GameObject.Find("Drone1.0").GetComponent<DroneCamera>();
        droneMovement = GameObject.Find("Drone1.0").GetComponent<DroneMovement>();
        clawGrabber = GameObject.Find("ClawHitBox").GetComponent<ClawGrabber>();
        TimerReset();
        blackScreen.enabled = false;
        screenSaverTexts.SetActive(false);
    }

    void FixedUpdate()
    {
        if (isTimerOn)
        {
            useDrone();
        }
        if (isBatteryRechargeFromZero == true)
        {
            rechargeBatteryFromZero();
        }

    }
    private void useDrone()
    {
        if (batteryTime > 0)
        {
            ChangeDroneMovement(isLookingAtTablet);
            blackScreen.enabled = false;
            if (droneMovement.getDroneVelocity() > 0.1)
            {
                batteryTime -= Time.deltaTime;
                batteryInner.fillAmount = batteryTime / batteryMaxTime;
            }
        }
        else
        {
            screenSaverTexts.SetActive(false);
            DisableDrone();
            droneMovement.changeDroneGravity(true);
            clawGrabber.ForceDropObject();
            droneMovement.resetToCheckpoint();
        }
    }
    private void rechargeBatteryFromZero()
    {
        if (batterRechargeTime > 0)
        {
            batterRechargeTime -= Time.deltaTime;
            if (batterRechargeTime < 0)
            {
                batterRechargeTime = 0;
            }
            screenOffTimer.text = String.Format("{0:0.##}", batterRechargeTime);
            isScreenSaverVisible();
        }
        else
        {
            batterRechargeTime = 0;
            removeScreenSaver();
            TimerReset();
        }
    }

    public void TimerReset()
    {
        //batteryFillIndicator.transform.localScale = new Vector3(.1f, batteryFillIndicator.transform.localScale.y, batteryFillIndicator.transform.localScale.z);
        isBatteryRechargeFromZero = false;
        batteryInner.fillAmount = 1;
        batteryTime = batteryMaxTime;
        isTimerOn = true;
        screenSaverTexts.SetActive(false);
        droneCamera.changeDroneState(isTimerOn);
        droneMovement.changeDroneState(isTimerOn);
        droneMovement.changeDroneGravity(false);
    }
    //Disable drone base on a different cooldown that can be used by other scripts
    public void DisableDrone(float cooldown)
    {
        batteryInner.fillAmount = 0;
        batteryTime = 0;
        blackScreen.enabled = true;
        isTimerOn = false;
        clawGrabber.ForceDropObject();
        droneCamera.changeDroneState(isTimerOn);
        droneMovement.changeDroneState(isTimerOn);
        screenSaverDroneTime(cooldown);
    }
    //Disable drone when the battery runs out set amount.
    private void DisableDrone()
    {
        batteryTime = 0;
        blackScreen.enabled = true;
        isTimerOn = false;
        droneCamera.changeDroneState(isTimerOn);
        droneMovement.changeDroneState(isTimerOn);
        screenSaverDroneTime(batteryCooldownTime);
    }
    //Stop Movement
    public void ChangeDroneMovement(bool droneState)
    {
        droneCamera.changeDroneState(droneState);
        droneMovement.changeDroneState(droneState);
    }
    //Start drone recharge timer
    private void screenSaverDroneTime(float delay)
    {
        if (isLookingAtTablet == true)
        {
            screenSaverTexts.SetActive(true);
        }
        batterRechargeTime = delay;
        isBatteryRechargeFromZero = true;
    }
    //Method for when tablet is being up or down to show the screen down time.
    public void isScreenSaverVisible()
    {
        if ((batterRechargeTime > 0) && isLookingAtTablet == true)
        {
            StartCoroutine(returnScreenSaver());
        }
        else if ((batterRechargeTime > 0) && isLookingAtTablet == false)
        {
            StopAllCoroutines();
            screenSaverTexts.SetActive(false);
        }
    }
    public void rechargeDroneBatteryFromAny()
    {
        if (isLookingAtTablet == false)
        {
            isTimerOn = false;
            if (batteryTime >= batteryMaxTime)
            {
                batteryTime = batteryMaxTime;
            }
            else
            {
                batteryTime += rechargeSpeed * Time.deltaTime;
            }
            batteryInner.fillAmount = batteryTime / batteryMaxTime;
        }
        else
        {
            isTimerOn = true;
        }
    }
    private void removeScreenSaver()
    {
        StopAllCoroutines();
        screenSaverTexts.SetActive(false);
    }
    public void setPlayerLookingAtTablet(bool lookingAtTablet)
    {
        this.isLookingAtTablet = lookingAtTablet;
    }
    IEnumerator returnScreenSaver()
    {
        yield return new WaitForSeconds(1);
        screenSaverTexts.SetActive(true);
    }
}
