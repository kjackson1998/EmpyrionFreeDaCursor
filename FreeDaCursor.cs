using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eleon.Modding;

namespace FreeDaCursor
{
    public class FreeDaCursorMod : IMod
    {
        private IModApi modApi;
        private UnityEngine.CursorLockMode? gameDesires;

        public void Init(IModApi modApi)
        {
            this.modApi = modApi;

            this.modApi.Application.Update += OnGameUpdate;
        }

        private void OnGameUpdate()
        {
            if (UnityEngine.Cursor.visible)
            {
                // If you can see the cursor, there is no reason to confine it!
                if (UnityEngine.Cursor.lockState != UnityEngine.CursorLockMode.None)
                {
                    // Empyrion wants it to be something, so store that so we can restore it
                    this.gameDesires = UnityEngine.Cursor.lockState;
                    UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
                }
            }
            else if (this.gameDesires.HasValue)
            {
                UnityEngine.Cursor.lockState = this.gameDesires.Value;
                this.gameDesires = null;
            }
        }

        public void Shutdown()
        {
        }
    }
}
