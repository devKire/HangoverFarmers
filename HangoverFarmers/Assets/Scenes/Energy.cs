using UnityEngine;
using TMPro;
using System;

public class EnergySystem : MonoBehaviour
{
    public int maxEnergy = 5;
    public float energyRechargeTime = 20f * 60f; // 20 minutos em segundos
    public TextMeshProUGUI energyText;

    private int currentEnergy;
    private DateTime lastEnergyUseTime;

    void Start()
    {
        currentEnergy = maxEnergy;
        lastEnergyUseTime = DateTime.Now;
        UpdateEnergyText();
    }

    void Update()
    {
        RechargeEnergy();
    }

    public void UseEnergy()
    {
        if (currentEnergy > 0)
        {
            currentEnergy--;
            lastEnergyUseTime = DateTime.Now;
            UpdateEnergyText();
        }
        else
        {
            Debug.Log("Sem energia suficiente!");
        }
    }

    private void RechargeEnergy()
    {
        TimeSpan timeSinceLastUse = DateTime.Now - lastEnergyUseTime;
        int energyToRecharge = (int)(timeSinceLastUse.TotalSeconds / energyRechargeTime);

        if (energyToRecharge > 0)
        {
            currentEnergy = Mathf.Min(maxEnergy, currentEnergy + energyToRecharge);
            lastEnergyUseTime = lastEnergyUseTime.AddSeconds(energyToRecharge * energyRechargeTime);
            UpdateEnergyText();
        }
    }

    private void UpdateEnergyText()
    {
        energyText.text = "Energia: " + currentEnergy.ToString();
    }

    public void StartGame()
    {
        if (currentEnergy > 0)
        {
            UseEnergy();
            Debug.Log("Jogo iniciado!");
            // Código para iniciar o jogo
        }
        else
        {
            Debug.Log("Você precisa de mais energia para iniciar o jogo.");
        }
    }
}
