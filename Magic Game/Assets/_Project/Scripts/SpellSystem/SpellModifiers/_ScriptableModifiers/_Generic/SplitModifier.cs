﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Split", menuName = "SpellSystem/Modifiers/Split")]
public class SplitModifier : SpellScriptableModifier
{

    [SerializeField] private int splitCount = 2;

    public override void AddSpellModifier(GameObject spellObject)
    {
        Split component = spellObject.AddComponent<Split>();
        component.splitCount = splitCount;
    }
}