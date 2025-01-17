﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnSelf : Spell
{

    // base class
    public List<OnStatusEffectRequirement> requirements = new List<OnStatusEffectRequirement>();
    protected Spellbook spellbook;

    //public override void CastSpell(Spellbook spellbook, int spellIndex)
    //{
    //
    //    ///<summary>
    //    ///
    //    ///                                 SELF SPELLS ARE DIFFERENT
    //    /// 
    //    ///     • self spells affect players directly so there is no need to instansiate the prefab
    //    ///     • on self spells work in a few ways
    //    ///         • They take instant effect on player (teleport a little distance forwards, etc.)
    //    ///         • They modify player stats directly (health, movementspeed, etc.) // heal, haste
    //    ///         • Give player a triple jump until he uses all of them ????
    //    ///         • Fix this shit --> no need to create instance just apply effects directly to player
    //    /// 
    //    /// </summary>
    //
    //    // TODO:: prototype of self castable spell
    //
    //    OnSelf self = Instantiate(this, spellbook.transform.position, spellbook.transform.rotation);
    //    self.transform.parent = spellbook.transform;
    //
    //    ApplyModifiers(self.gameObject, spellIndex, spellbook);
    //
    //    spellbook.StopCasting();
    //
    //}

    public void Validate()
    {

        spellbook = GetComponent<Spellbook>();

        List<OnStatusEffectRequirement> requirementsCopy = new List<OnStatusEffectRequirement>(requirements);
        foreach (OnStatusEffectRequirement requirement in requirementsCopy)
        {
            if(!requirement.isMet(spellbook))
            {
                print("Requirement: " + requirement + " was not met");
                RemoveEffect();
            }
        }

    }

    public abstract void RemoveEffect();

}
