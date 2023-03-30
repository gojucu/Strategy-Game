using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class HealthModel : MonoBehaviour
{
    [SerializeField]
    private float currentHealth;
    private float maxHealth;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
        }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = Mathf.Max(0, value);
        }
    }


    public void SetHealthModel(float startingValue,float maxHealth)
    {
        CurrentHealth = startingValue;
        MaxHealth = maxHealth;
    }
}
