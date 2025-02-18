﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : SpellModifier
{

    public GameObject portalGatePrefab;
    public int maximumPortals = 2;
    public float duration = 5;

    private void Start()
    {
        PortalGateManager.Instance.maximumPortals = maximumPortals;
        PortalGateManager.Instance.SpellDuration = duration;
        PortalGateManager.Instance.UpdateFromModifier();
    }

    public override void OnSpellCast(Spell spell)
    {
        if(spell.spellType == SpellType.AOE)
        {
            GameObject portalgate = Instantiate(portalGatePrefab, spell.caster.transform.position, spell.caster.transform.rotation);
            PortalGateManager.Instance.CreatePortalGate(portalgate);
        }
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        GameObject portalgate = Instantiate(portalGatePrefab, collision.contacts[0].point, Quaternion.identity);
        PortalGateManager.Instance.CreatePortalGate(portalgate);
    }

}
