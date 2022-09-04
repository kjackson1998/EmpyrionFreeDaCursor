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
        private bool doingOverride;
        private UnityEngine.CursorLockMode lastKnownGamePreference;

        public void Init(IModApi modApi)
        {
            this.modApi = modApi;

            this.modApi.Application.Update += OnGameUpdate;
        }

        private void OnGameUpdate()
        {
            if (!this.doingOverride
                || UnityEngine.Cursor.lockState != UnityEngine.CursorLockMode.None)
            {
                // If we are not overriding, or the cursor is for any reason confined,
                // then record the game's preference
                this.lastKnownGamePreference = UnityEngine.Cursor.lockState;
            }

            if (this.doingOverride)
            {
                // If we are overriding, check to see if the cursor was hidden.
                if (!UnityEngine.Cursor.visible)
                {
                    // Time to restore what the game wants, since they hid the cursor
                    // and try to avoid being heavy handed calling setters if we don't need to.
                    // Never know who's watching!
                    if (UnityEngine.Cursor.lockState != this.lastKnownGamePreference)
                        UnityEngine.Cursor.lockState = this.lastKnownGamePreference;
                    this.doingOverride = false;
                }
            }
            else
            {
                // If we aren't overriding, check if we need to
                if (UnityEngine.Cursor.visible
                    && UnityEngine.Cursor.lockState != UnityEngine.CursorLockMode.None)
                {
                    // Game wants to confine cursor AND show the cursor. Unless the game does
                    // edge panning, this is just really poor UX. Empyrion I do not believe
                    // does any edge panning.
                    // So, let's take over (or re-insist if already have taken over) for poor design.
                    UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
                    this.doingOverride = true;
                }
            }
        }

        public void Shutdown()
        {
        }
    }
}
