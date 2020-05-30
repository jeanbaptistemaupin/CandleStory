﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]

public class FPSInputController : MonoBehaviour
{
    private CharacterMotor motor;

    public GameObject flashLight;
    public bool flashLightOn = false;

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        if (Checkpoint.checkpointReached == true)
        {
            CharacterMotor.controller.enabled = false;
            gameObject.transform.position = GameManager.CheckpointPosition;
            CharacterMotor.controller.enabled = true;
        }
    }

    void Update()
    {
        // Get the input vector from kayboard or analog stick
        Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (directionVector != Vector3.zero)
        {
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1.0f, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength *= directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector *= directionLength;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(flashLightOn == false)
            {
                flashLight.SetActive(true);
                flashLightOn = true;
            }
            else
            {
                flashLight.SetActive(false);
                flashLightOn = false;
            }
        }

        // Apply the direction to the CharacterMotor
        motor.input.direction = transform.rotation * directionVector;
        motor.input.jump = Input.GetButton("Jump");
    }
}
