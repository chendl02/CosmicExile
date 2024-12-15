using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageController
{
    public static void loadStage(int stage)
    {
        stage -= 1;
        // �ҵ���ǰ��������Ϊ "Stages" �� GameObject
        GameObject stagesObject = GameObject.Find("Stages");
        if (stagesObject == null)
        {
            Debug.LogError("Stages GameObject not found in the scene!");
            return;
        }

        // ��� "Stages" �Ƿ����㹻���Ӷ���
        if (stage < 0 || stage >= stagesObject.transform.childCount)
        {
            Debug.LogError("Invalid stage index!");
            return;
        }

        // ��ȡ�� stage ���Ӷ���
        Transform stageTransform = stagesObject.transform.GetChild(stage);
        if (stageTransform == null)
        {
            Debug.LogError("Child stage not found!");
            return;
        }

        // ��ȡ�Ӷ����ϵ� SpaceState �ű�
        SpaceState spaceState = stageTransform.GetComponent<SpaceState>();
        if (spaceState == null)
        {
            Debug.LogError($"SpaceState script not found on child stage {stage}!");
            return;
        }

        // �ɹ���ȡ SpaceState�����������������в���
        Debug.Log($"Successfully loaded SpaceState from stage {stage}");

        Clock.dayTime = spaceState.dayTime;


    }
}
