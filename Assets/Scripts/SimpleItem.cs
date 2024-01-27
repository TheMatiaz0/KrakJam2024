using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrakJam2024
{
    public class SimpleItem : Item
    {
        public override void Use(PlayerOwnerTransmitter owner)
        {
            var playerGO = owner.Player.gameObject;
            // ...
        }
    }
}
