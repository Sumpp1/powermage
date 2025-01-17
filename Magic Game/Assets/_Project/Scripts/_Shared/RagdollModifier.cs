﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RagdollModifier : MonoBehaviour
{
    [SerializeField] private string armatureName = "Armature";
    [SerializeField] private List<Rigidbody> excludeFromKinematicToggle = new List<Rigidbody>();
    [SerializeField] private List<Transform> hiddenWhenNotKinematic = new List<Transform>();

    private List<Transform> armatureBones = new List<Transform>();
    private Vector3[] armatureDefaultPosition = null;
    private Quaternion[] armatureDefaultRotation = null;
    private Vector3[] armatureDefaultScale = null;

    void Start()
    {
        GetBones(armatureBones, armatureName);

        armatureDefaultPosition = new Vector3[armatureBones.Count];
        armatureDefaultRotation = new Quaternion[armatureBones.Count];
        armatureDefaultScale = new Vector3[armatureBones.Count];

        for (int i = 0; i < armatureBones.Count; i++)
        {
            armatureDefaultPosition[i] = armatureBones[i].localPosition;
            armatureDefaultRotation[i] = armatureBones[i].localRotation;
            armatureDefaultScale[i] = armatureBones[i].localScale;
        }

        SetDepenetrationValues(armatureBones, 3.0f);
        SetKinematic(true, false);
    }

    void GetBones(List<Transform> list, string armatureName)
    {
        list.Clear();

        foreach (Transform item in transform)
        {
            if (item.name == armatureName)
            {
                Debug.Log("Found the armature, looping through all of its child transforms...");
                GetAllChildren(item, armatureBones);
                Debug.Log("Found " + armatureBones.Count + " bones.");
            }
        }
    }

    void GetAllChildren(Transform parent, List<Transform> list)
    {
        foreach (Transform item in parent)
        {
            list.Add(item);
            if (item.childCount > 0)
            {
                GetAllChildren(item, list);
            }
        }
    }

    void SetDepenetrationValues(List<Transform> list, float amount)
    {
        if (list.Count > 0)
        {
            foreach (Transform item in list)
            {
                Rigidbody rigid = item.GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    rigid.maxDepenetrationVelocity = amount;
                }
            }
            Debug.Log("Set ragdoll's rigidbodies' maxDepenetrationVelocity to " + amount + ".");
        }
    }

    public void SetKinematic(bool setKinematic, bool detectCollisions)
    {
        SetKinematic(setKinematic, detectCollisions, armatureBones);
    }

    void SetKinematic(bool setKinematic, bool detectCollisions, List<Transform> list)
    {
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].localPosition = armatureDefaultPosition[i];
                list[i].localRotation = armatureDefaultRotation[i];
                list[i].localScale = armatureDefaultScale[i];

                Rigidbody rigid = list[i].GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    if (!excludeFromKinematicToggle.Contains(rigid))
                    {
                        rigid.isKinematic = setKinematic;
                        rigid.detectCollisions = detectCollisions;
                    }
                }
            }
            Debug.Log("Set ragdoll's rigidbodies' isKinematic to " + (setKinematic ? "true." : "false."));
        }

        foreach (Transform item in hiddenWhenNotKinematic)
        {
            item.gameObject.SetActive(setKinematic);
        }
    }
}
