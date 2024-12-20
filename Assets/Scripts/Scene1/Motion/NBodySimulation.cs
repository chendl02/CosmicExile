﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MotionData
{
    public Vector3 Position;
    public Vector3 Velocity;

    public MotionData(Vector3 position, Vector3 velocity)
    {
        Position = position;
        Velocity = velocity;
    }
}

public class NBodySimulation : MonoBehaviour {
    public CelestialBody[] bodies;
    //static NBodySimulation instance;

    void Awake () {
        //instance = null;
        bodies = FindObjectsOfType<CelestialBody> ();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void FixedUpdate () {
        if (Clock.speed == 0)
            return;

        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdatePosition();
        }

        Cursor.visible = true;

    }

    public static MotionData RK4(MotionData data, float dayTime, float timeStep)
    {
        Vector3 k1v, k2v, k3v, k4v;
        Vector3 k1p, k2p, k3p, k4p;

        Vector3 position = data.Position;
        Vector3 velocity = data.Velocity;

        k1v = timeStep * CalculateAcceleration(position, dayTime);
        k1p = timeStep / Universe.distanceCoefficint * velocity;

        k2v = timeStep * CalculateAcceleration(position + 0.5f * k1p, dayTime + 0.5f * timeStep / 3600 / 24);
        k2p = timeStep / Universe.distanceCoefficint * (velocity + 0.5f * k1v);

        k3v = timeStep * CalculateAcceleration(position + 0.5f * k2p, dayTime + 0.5f * timeStep / 3600 / 24);
        k3p = timeStep / Universe.distanceCoefficint * (velocity + 0.5f * k2v);

        k4v = timeStep * CalculateAcceleration(position + k3p, dayTime + timeStep / 3600 / 24);
        k4p = timeStep / Universe.distanceCoefficint * (velocity + k3v);

        position += (k1p + 2 * k2p + 2 * k3p + k4p) / 6;
        velocity += (k1v + 2 * k2v + 2 * k3v + k4v) / 6;

        return new MotionData(position, velocity);
    }


    public static Vector3 CalculateAcceleration (Vector3 point, float dayTime, CelestialBody ignoreBody = null)
    {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in FindObjectOfType<NBodySimulation>().bodies)
        {
            if (body != ignoreBody)
            {
                float sqrDst = (body.GetRealPosition(dayTime) - point).sqrMagnitude;
                Vector3 forceDir = (body.GetRealPosition(dayTime) - point).normalized;
                acceleration += forceDir * Universe.gravitationalConstant * body.mass / sqrDst;
            }
        }

        return acceleration;
    }

    public static void ActivateVirtualMesh(float dayTime)
    {
        foreach (var body in FindObjectOfType<NBodySimulation>().bodies)
        {
            body.ActivateVirtualMesh(body.GetRealPosition(dayTime));
        }
    }
    public static void DeactivateVirtualMesh()
    {
        foreach (var body in FindObjectOfType<NBodySimulation>().bodies)
        {
            body.DeactivateVirtualMesh();
        }
    }
    public static void DrawPredict(int days)
    {
        foreach (var body in FindObjectOfType<NBodySimulation>().bodies)
        {
            body.DrawPredict(days);
        }
    }

    public static void UpdatePredict(float dayTime)
    {
        foreach (var body in FindObjectOfType<NBodySimulation>().bodies)
        {
            body.updatePredict(dayTime);
        }
    }
    public static void DrawOrbit()
    {
        foreach (var body in FindObjectOfType<NBodySimulation>().bodies)
        {
            body.DrawOrbit();
        }
    }
}