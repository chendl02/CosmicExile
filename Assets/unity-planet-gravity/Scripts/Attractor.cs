﻿/*******************************************************************************************
* Author: German L.G Fica
* Websites: http://germanfica.xyz
* Description: Basic gravity attractor.
*******************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xyz.germanfica.unity.planet.gravity
{
    public class Attractor : MonoBehaviour
    {
        public static List<Attractor> Attractors;
        public float gravity = -10;
        private Transform m_Transform;
        private Rigidbody m_Rigidbody;

        public bool is_star;

        /* Apply gravity to the game object
         */
        public void Attract(Transform body)
        {
            float distance = Vector3.Distance(body.position, transform.position);
            if (distance > 5000f) { return; }
            Vector3 gravityUp = (body.position - transform.position).normalized;
            Vector3 bodyUp = body.up;
            body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
            Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;

            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
        }

        /* All necessary variables to use gravity are initialized 
         */
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            m_Rigidbody.useGravity = false;

            m_Transform = GetComponent<Transform>();
        }

        /* Applies gravity to game objects associated with the script
         * 
         * Note: each game object can have its own gravity
         */
        void FixedUpdate()
        {
            if (m_Rigidbody != null && m_Transform != null)
            {
                foreach (Attractor attractor in Attractors)
                {
                    if (attractor != this&&!is_star)
                        attractor.Attract(m_Transform);
                        Debug.Log("Attracting: " + m_Transform.name + " to " + attractor.name);
                }
            }
        }

        /* Add this game object to the list
         */
        void OnEnable()
        {
            if (Attractors == null)
                Attractors = new List<Attractor>();

            Attractors.Add(this);
            Debug.Log("Added to Attractors list: " + gameObject.name);
        }

        /* When disabled, it must be deleted from the list
         */
        void OnDisable()
        {
            Attractors.Remove(this);
        }
    }
}