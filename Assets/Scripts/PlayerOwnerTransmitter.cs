using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrakJam2024
{
    public class PlayerOwnerTransmitter : MonoBehaviour
    {
        [SerializeField]
        private PlayerItemHolder _player;

        public PlayerItemHolder Player => _player;
    }
}
