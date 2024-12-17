using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class StageController
{
    public static int stage = 0;
    public static bool stageStart = false;
    public static void NextStage()
    {
        stageStart = true;
        stage += 1;
    }

    public static void NextStage(int nextStage)
    {
        stageStart = true;
        stage = nextStage;
    }

    public static void LoadStage()
    {
        LoadStage(stage);
    }

    public static void LoadStage(int stage)
    {
        stage -= 1;
        // 找到当前场景中名为 "Stages" 的 GameObject
        GameObject stagesObject = GameObject.Find("Stages");
        if (stagesObject == null)
        {
            Debug.LogError("Stages GameObject not found in the scene!");
            return;
        }

        // 检查 "Stages" 是否有足够的子对象
        if (stage < 0 || stage >= stagesObject.transform.childCount)
        {
            Debug.LogError("Invalid stage index!");
            return;
        }

        // 获取第 stage 个子对象
        Transform stageTransform = stagesObject.transform.GetChild(stage);
        if (stageTransform == null)
        {
            Debug.LogError("Child stage not found!");
            return;
        }

        // 获取子对象上的 SpaceState 脚本
        SpaceState spaceState = stageTransform.GetComponent<SpaceState>();
        if (spaceState == null)
        {
            Debug.LogError($"SpaceState script not found on child stage {stage}!");
            return;
        }

        // 成功获取 SpaceState，可以在这里对其进行操作
        Debug.Log($"Successfully loaded SpaceState from stage {stage}");

        Clock.dayTime = spaceState.dayTime;

        Ship ship = GameObject.Find("Space Ship").GetComponent<Ship>();
        ship.initVelocity = spaceState.velocity;
        ship.initPosition = spaceState.position;
        ship.motionData = new MotionData(spaceState.position, spaceState.velocity);
        ship.transform.position = spaceState.position;

        TextMeshProUGUI target = GameObject.Find("Target").GetComponent<TextMeshProUGUI>();
        target.text = "Target: " + spaceState.target;

    }
}
