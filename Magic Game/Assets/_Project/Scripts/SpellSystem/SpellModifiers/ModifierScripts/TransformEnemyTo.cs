﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformEnemyTo : SpellModifier
{

    public GameObject transformPrefab { get; set; }
    public float duration;
    public GameObject transformationParticles { get; set; }

    public override void AoeCollide(GameObject hitObject)
    {
        InitTransformation(hitObject);
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {
        InitTransformation(hitInfo.collider.gameObject);
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        InitTransformation(collision.gameObject);
    }

    private void InitTransformation(GameObject orginal)
    {
        if(orginal.GetComponent<EnemyCore>())
        {
            if (orginal.GetComponent<Rigidbody>() != null && orginal.transform.GetComponent<Transformation>() == null && orginal.transform.parent == null)
            {
                orginal.SetActive(false);

                if (transformPrefab != null)
                {
                    Transformation tempTransform = Instantiate(transformPrefab, orginal.transform.position, Quaternion.identity).GetComponent<Transformation>();
                    tempTransform.TransformedObject = orginal;
                    tempTransform.duration = duration;
                    tempTransform.transformationParticles = transformationParticles;
                }

                if (transformPrefab != null)
                {
                    Instantiate(transformationParticles, orginal.transform.position, Quaternion.identity);
                }
            }
        }
    }

}
