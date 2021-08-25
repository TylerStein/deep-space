using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIController : MonoBehaviour
{
    public ZBHPlayerHealthController zbhPlayerHealth;
    public ZBHCoreController zbhCore;
    public UIBarController uiPlayerBar;
    public UIBarController uiBossBar;

    // Start is called before the first frame update
    private void Awake()
    {
        uiPlayerBar.SetMaxTicks(zbhPlayerHealth.healthPoints);
        uiBossBar.SetMaxTicks(zbhCore.healthPoints);

        zbhPlayerHealth.hpLostEvent.AddListener(onEvent_updatePlayerHealth);
        zbhCore.hpChangedEvent.AddListener(onEvent_updateCoreHealth);
    }

    private void OnDestroy() {
        zbhPlayerHealth.hpLostEvent.RemoveListener(onEvent_updatePlayerHealth);
        zbhCore.hpChangedEvent.RemoveListener(onEvent_updateCoreHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onEvent_updatePlayerHealth() {
        uiPlayerBar.SetActiveTicks(zbhPlayerHealth.healthPoints);
    }

    void onEvent_updateCoreHealth() {
        uiBossBar.SetActiveTicks(zbhCore.healthPoints);
    }
}
