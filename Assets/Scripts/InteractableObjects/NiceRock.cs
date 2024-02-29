using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plattko
{
    public class NiceRock : InteractRadius
    {
        public override void Interact()
        {
            Debug.Log("Nice.");
        }
    }
}
