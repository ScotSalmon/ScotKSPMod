using UnityEngine;

namespace ScotKSPMod
{
    public class HelloKerbinMod : PartModule
    {
        private float lastFixedUpdateLogTime = 0.0f;
        private float logInterval = 5.0f;
        private float torqueSoFar = 0.0f;
        private float totalTorqueApplicationTime = 20.0f;
        private Rigidbody _rigidbody;

        [KSPField(guiActive = true, guiName = "Torque", isPersistant = true)]
        [UI_FloatRange(maxValue = 10000.0f, minValue = 0.0f, scene = UI_Scene.Editor, stepIncrement = 100.0f)]
        private float totalTorque = 10000.0f;

        [KSPField(guiActive = true, guiName = "Activate", isPersistant = true)]
        [UI_Toggle(controlEnabled = true, scene = UI_Scene.All)]
        private bool applyTorque = false;

        public HelloKerbinMod()
        {
            Debug.Log("HelloKerbinMod constructor");
        }

        public override void OnAwake()
        {
            Debug.Log("HelloKerbinMod Awake");
        }

        void FixedUpdate()
        {
            if ((Time.time - lastFixedUpdateLogTime) > logInterval)
            {
                lastFixedUpdateLogTime = Time.time;
                Debug.Log("HelloKerbinMod FixedUpdate: torqueSoFar = " + torqueSoFar);
            }

            if ((this.vessel == FlightGlobals.ActiveVessel) && applyTorque)
            {
                var torqueThisTime = (Time.deltaTime / totalTorqueApplicationTime) * totalTorque;
                if (torqueThisTime + torqueSoFar > totalTorque)
                {
                    applyTorque = false;
                    torqueThisTime = totalTorque - torqueSoFar;
                }
                Rigidbody rb = this.vessel.GetComponentCached(ref _rigidbody);
                rb.AddRelativeTorque(0, torqueThisTime, 0);
                torqueSoFar += torqueThisTime;
            }
        }

        void OnDestroy()
        {
            Debug.Log("HelloKerbinMod OnDestroy");
        }

        [KSPAction("Spin")]
        public void DoSpin(KSPActionParam p)
        {
            applyTorque = true;
        }
    }
}
