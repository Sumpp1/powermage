﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffect
{

    [HideInInspector] public string name;
    [HideInInspector] public GameObject target;
    [HideInInspector] public StatusEffectManager effectManager;
    [HideInInspector] public float endTime;

    public GameObject graphics;
    public float duration;

    protected GameObject graphicsCopy;

    public GameObject projecttileElementGraphic;
    public Beam.ElementType beamElementGraphic;
    public GameObject aoeElementGraphic;

    public GameObject projectileExplosionGraphic;

    public float extraManaCost = 0f;

    public Mana playerMana;

    #region Cloning
    public abstract StatusEffect Clone();
    /*
    {
        
        StatusEffect temp = new StatusEffect(duration, graphics);
        temp.name = name;
        temp.target = target;
        temp.effectManager = effectManager;
        temp.endTime = endTime;
        temp.graphicsCopy = graphicsCopy;
        temp.projecttileElementGraphic = projecttileElementGraphic;
        temp.beamElementGraphic = beamElementGraphic;
        temp.aoeElementGraphic = aoeElementGraphic;
        return temp;
        
    }
    */
    #endregion

    // StatusEffectManager uses this
    public bool IsFinished
    {
        get { return Time.time > endTime; }
    }

    // Will be inherited with more parameters
    public StatusEffect(float duration, GameObject graphics) 
    {
        this.duration = duration;
        this.graphics = graphics;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerMana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
        }
    }

    // This will be called from each entitys own StatusEffectManager when the effect is about to be applied
    public virtual void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {
        Debug.Log("calling base apply");

        endTime = Time.time + duration;
        this.target = target;
        effectManager = target.GetComponent<StatusEffectManager>();
        graphicsCopy = GameObject.Instantiate(graphics, target.transform.position + Vector3.up , Quaternion.FromToRotation(-graphics.transform.up, Vector3.up));
        graphicsCopy.transform.SetParent(target.transform);
    }

    // OnTick is used by effects like ignite / heal over time that need to be updated while applied to target
    public virtual void OnTick() { }

    // StatusEffectManager (on entity) calls OnLeave when the duration is passed or the countering effect is applied to the manager as new
    public virtual void OnLeave()
    {
        if (target != null && graphicsCopy != null)
        {
            GameObject.Destroy(graphicsCopy.gameObject);
        }
        else
        {
            Debug.Log("target is null");
        }
    }

    // esim. Moisturize overrides this and spawns water pool when hitting something that doesn't have health
    public virtual void HitNonlivingObject(Collision collision) { }

    // Refresh duration of effect and check for countering effects
    public virtual void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        CheckForCounterEffects(allEffectsInSpell);
        endTime = Time.time + duration;
    }

    // Ignite and moisturize use this to check the existing StatusEffect and what are new effects in spell
    public virtual void CheckForCounterEffects(List<StatusEffect> allEffectsInSpell) { }

    public void SetElementParticles(GameObject projecttileParticle, Beam.ElementType beamParticle, GameObject aoeParticle)
    {
        projecttileElementGraphic = projecttileParticle;
        beamElementGraphic = beamParticle;
        aoeElementGraphic = aoeParticle;
    }

    public void SetProjectileExplosion(GameObject projectileParticle)
    {
        projectileExplosionGraphic = projectileParticle;
    }

    public bool isEffectable()
    {
        return (extraManaCost < playerMana.mana);
    }

}
