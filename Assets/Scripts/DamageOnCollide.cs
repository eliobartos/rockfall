using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollide : MonoBehaviour
{
    // The amount of damage we will deal to anything we hit
    public int damage = 1;

    // The amount of damage we deal to ourselves when we hit something
    public int damageToSelf = 5;

    void HitObject(GameObject theObject) {
        Debug.Log("Object: " + gameObject.name + " hit " + theObject.name);

        // If hit object is ship (player) <> shot, do nothing
        // If it is shot, destroy it so it does not damages 2 times space station
        // Because of the space station shape it can enter it 2 times
        if( (this.gameObject.tag == "Player" && theObject.tag == "Shot") ||  (this.gameObject.tag == "Shot" && theObject.tag == "Player")) {
            return;
        } else if(this.gameObject.tag == "Shot") {
            Destroy(this.gameObject);
        } else if(theObject.tag == "Shot") {
            Destroy(theObject);
        }

        // Do damage to the thing we hit, if possible
        var theirDamage = theObject.GetComponentInParent<DamageTaking>();
        if(theirDamage) {
            theirDamage.TakeDamage(damage);
        }

        // Do damage to ourself, if possible
        var ourDamage = this.GetComponentInParent<DamageTaking>();
        if(ourDamage) {
            ourDamage.TakeDamage(damageToSelf);
        }
    }

    // Did an object enter this trigger area?
    void OnTriggerEnter(Collider collider) {
        HitObject(collider.gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        HitObject(collision.gameObject);
    }
}
