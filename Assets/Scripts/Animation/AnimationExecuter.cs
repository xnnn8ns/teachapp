﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))] 
public class AnimationExecuter : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    public void StartLeftRightTurn()
    {
        StartCoroutine(LeftRightTurn());
    }

    public void StartUpDownTurn()
    {
        StartCoroutine(UpDownTurn());
    }

    private IEnumerator LeftRightTurn()
    {
        int counter = 0;
        bool isToLeft = true;
        int countCircle = 3;
        int countIteration = 6;
        Quaternion startQuaternion = _transform.rotation;
        Vector3 startQuaternionEuler = startQuaternion.eulerAngles;
        Vector3 targetLeftQuaternionEuler = startQuaternionEuler - Vector3.up * 10f;
        Quaternion targetLeftQuaternion = Quaternion.Euler(targetLeftQuaternionEuler);
        Vector3 targetRightQuaternionEuler = startQuaternionEuler - Vector3.down * 10f;
        Quaternion targetRightQuaternion = Quaternion.Euler(targetRightQuaternionEuler);
        Quaternion targetQuaternion = targetLeftQuaternion;
        float speed = 1/10f;
        while (counter < countIteration)
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, targetQuaternion, speed);
            yield return new WaitForFixedUpdate();
            counter++;
            if (counter == countIteration)
            {
                if (isToLeft)
                    targetQuaternion = targetRightQuaternion;
                else
                    targetQuaternion = targetLeftQuaternion;

                countCircle--;
                isToLeft = !isToLeft;

                if (countCircle == 1)
                {
                    counter = 0;
                    targetQuaternion = startQuaternion;
                    speed *= 2;
                }
                else
                {
                    counter = -countIteration;  
                }
                yield return new WaitForSeconds(0.1f);
            }
            if (countCircle <= 0)
                break;
        }
        _transform.rotation = startQuaternion;
    }

    private IEnumerator UpDownTurn()
    {
        int counter = 0;
        bool isToUp = true;
        int countCircle = 3;
        int countIteration = 6;
        Quaternion startQuaternion = _transform.rotation;
        Vector3 startQuaternionEuler = startQuaternion.eulerAngles;
        Vector3 targetUpQuaternionEuler = startQuaternionEuler - Vector3.left * 10f;
        Quaternion targetUpQuaternion = Quaternion.Euler(targetUpQuaternionEuler);
        Vector3 targetDownQuaternionEuler = startQuaternionEuler - Vector3.right * 10f;
        Quaternion targetDownQuaternion = Quaternion.Euler(targetDownQuaternionEuler);
        Quaternion targetQuaternion = targetUpQuaternion;
        float speed = 1 / 10f;
        while (counter < countIteration)
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, targetQuaternion, speed);
            yield return new WaitForFixedUpdate();
            counter++;
            if (counter == countIteration)
            {
                if (isToUp)
                    targetQuaternion = targetDownQuaternion;
                else
                    targetQuaternion = targetUpQuaternion;

                countCircle--;
                isToUp = !isToUp;

                if (countCircle == 1)
                {
                    counter = 0;
                    targetQuaternion = startQuaternion;
                    speed *= 2;
                }
                else
                {
                    counter = -countIteration;
                }
                yield return new WaitForSeconds(0.1f);
            }
            if (countCircle <= 0)
                break;
        }
        _transform.rotation = startQuaternion;
    }
}
