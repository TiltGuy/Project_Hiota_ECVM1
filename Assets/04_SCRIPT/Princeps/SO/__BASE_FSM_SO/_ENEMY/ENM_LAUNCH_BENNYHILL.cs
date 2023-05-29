using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENM_LAUNCH_BENNYHILL : Action_SO
{
    public override void Act( Controller_FSM controller )
    {
        controller.BrainAI.LaunchBennyHillTimer();
    }
}
