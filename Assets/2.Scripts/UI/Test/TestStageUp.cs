using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStageUp : MonoBehaviour
{
    public void StageUp()
    {
        Managers.Stage.TestUpStage();
    }

    public void StageClear()
    {
        Managers.Stage.Resert();
    }
}
