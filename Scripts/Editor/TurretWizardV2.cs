using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

/// <summary>
/// Automatically assigns necessary classes to correctly defined model. Model needs to be manually updated to assign RangeChecker, HealthSlider, ShieldSlider.
/// </summary>
public class TurretWizardV2 : ScriptableWizard
{
    private Dictionary<string, VariableReference> turretComponentPairs = new Dictionary<string, VariableReference>();
    public Transform horizontalRotation;
    public Transform verticalRotation;
    public Transform barrelRotation;
    public List<GameObject> projectileSpawns;
    public SphereCollider range;
    public Slider healthSlider;
    public Slider shieldSlider;
    GameObject tempSpawner;


    [MenuItem("Custom Tools/Turret wizard v2")]
    static void CreateWizard()
    {
        // Title, Button name, other button
        ScriptableWizard.DisplayWizard<TurretWizardV2>("Create turret v2", "Create new", "Update selected");
    }
    // TODO: Fully automate component assignment
    void OnWizardUpdate()
    {
        Debug.Log("Autofill v2 scan running...");
        if (turretComponentPairs.Count <= 0)
        {
            turretComponentPairs.Add("Horizontal", new VariableReference(() => horizontalRotation, hr => { horizontalRotation = (Transform)hr; }, typeof(Transform)));
            turretComponentPairs.Add("Vertical", new VariableReference(() => verticalRotation, vr => { verticalRotation = (Transform)vr; }, typeof(Transform)));
            turretComponentPairs.Add("Barrel", new VariableReference(() => barrelRotation, br => { barrelRotation = (Transform)br; }, typeof(Transform)));
            turretComponentPairs.Add("RangeChecker", new VariableReference(() => range, rc => { range = (SphereCollider)rc; }, typeof(SphereCollider)));
            turretComponentPairs.Add("HealthSlider", new VariableReference(() => healthSlider, hs => { healthSlider = (Slider)hs; }, typeof(Slider)));
            turretComponentPairs.Add("ShieldSlider", new VariableReference(() => shieldSlider, ss => { shieldSlider = (Slider)ss; }, typeof(Slider)));
            // Every turret need at lest one Spawner, if it has more continue scan until null
            //turretComponentPairs.Add("Spawner", new VariableReference(() => tempSpawner, ps => { tempSpawner = (GameObject)ps; }, typeof(Transform)));

            // TODO: Spawner
            Transform t = Selection.activeTransform;
            int count = t.childCount;
            AutoFillWizardFields(count, t);
        }
    }
    // Recursive scan for the object children
    void AutoFillWizardFields(int count, Transform t)
    {
        // Recursive loop
        for (int i = 0; i < count; i++)
        {
            Transform temp = t.GetChild(i);
            int tcount = temp.childCount;
            // TODO: Implement spawner
            //Debug.Log(temp.name + " child " + tcount + " type: " + temp.GetType());
           /* if(temp.name.Contains("Spawner"))
            {
                Debug.Log("Spawner found");
            }*/
            AssignEmptyFields(temp);
            if (tcount >= 1)
            {
                AutoFillWizardFields(tcount, temp);
            }
        }
    }

    void AssignEmptyFields(Transform t)
    {
        if (turretComponentPairs.ContainsKey(t.name))
        {
            bool isTypeInComponent = true;
            // If current object is of same type as object we are searching for
            if (turretComponentPairs[t.name].GetCustType.Equals(typeof(Transform)))     // Could also use ..Equals(t.GetType())
            {
                isTypeInComponent = false;
                //Debug.Log(t.name + " of type " + t.GetType() + " is same as " + turretComponentPairs[t.name].GetCustType);
                turretComponentPairs[t.name].Set(t);
            }
            // If current object name is matching but its type is different then search its components for matching object
            if (isTypeInComponent)
            {
                // Assigns the first component found in the object of t type
                turretComponentPairs[t.name].Set(t.GetComponent(turretComponentPairs[t.name].GetCustType));
            }
        }
    }

    void OnWizardCreate()
    {
        GameObject turret = new GameObject();
        turret.name = "Turret_";

        Rigidbody rigidbody = turret.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    void OnWizardOtherButton()
    {
        // If we have something selected
        if (Selection.activeTransform != null)
        {
            Rigidbody rigidbody = Selection.activeTransform.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = Selection.activeGameObject.AddComponent<Rigidbody>();
            }
            rigidbody.useGravity = false;
            rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            // TurretInit requires TurretAI which requires RangeChecker, TurretTrackingV3, Weapon
            TurretInit turretInit = Selection.activeTransform.GetComponent<TurretInit>();
            if (turretInit == null)
            {
                turretInit = Selection.activeGameObject.AddComponent<TurretInit>();
            }

            TurretTrackingV4 tracking = Selection.activeTransform.GetComponent<TurretTrackingV4>();
            if (tracking == null)
            {
                tracking = Selection.activeGameObject.AddComponent<TurretTrackingV4>();
            }
            tracking.horizontalRotation = horizontalRotation;
            tracking.verticalRotation = verticalRotation;
            tracking.barrelRotation = barrelRotation;

            range = Selection.activeTransform.Find("RangeChecker").GetComponent<SphereCollider>();
            RangeChecker rangeChecker = Selection.activeTransform.GetComponent<RangeChecker>();
            rangeChecker.range = range;

            healthSlider = healthSlider.GetComponent<Slider>();
            HealthUI healthUI = Selection.activeTransform.GetComponent<HealthUI>();
            healthUI.slider = healthSlider;

            Weapon weapon = Selection.activeTransform.GetComponent<Weapon>();
            weapon.projectileSpawns = projectileSpawns;

            MessageHandler messageHandler = Selection.activeTransform.GetComponent<MessageHandler>();
            if(!messageHandler.messages.Contains(MessageType.HEALTHCHANGED))
            {
                messageHandler.messages.Add(MessageType.HEALTHCHANGED);
            }
            if (!messageHandler.messages.Contains(MessageType.DAMAGED))
            {
                messageHandler.messages.Add(MessageType.DAMAGED);
            }
            if (!messageHandler.messages.Contains(MessageType.DIED))
            {
                messageHandler.messages.Add(MessageType.DIED);
            }
        }
    }
}
