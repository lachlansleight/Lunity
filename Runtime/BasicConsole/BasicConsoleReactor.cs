using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public class BasicConsoleReactor : MonoBehaviour
    {
        public virtual void ReceiveCommand(string command, params string[] args)
        {
            Debug.Log("Received command: " + command + " with args " + args.Join(", "));
        }
    }
}