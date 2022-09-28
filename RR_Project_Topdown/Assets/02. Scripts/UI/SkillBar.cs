using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    public Slider slider;
    public Text healthtext;
    private float maxhealth;

    public void SetMaxSkill(float skill)
    {
        slider.maxValue = skill;
        slider.value = skill;
        healthtext.text = (skill.ToString() + "/" + skill.ToString());

    }

    public void AddSkill(float add)
    {
        slider.value += add;
        maxhealth = slider.maxValue;
        healthtext.text = (System.Convert.ToInt32(slider.value).ToString() + "/" + maxhealth.ToString());
    }


    public void SetSkill(float skill)
    {
        slider.value = skill;
        maxhealth = slider.maxValue;
        healthtext.text = (skill.ToString() + "/" + maxhealth.ToString());
    }

    
}
