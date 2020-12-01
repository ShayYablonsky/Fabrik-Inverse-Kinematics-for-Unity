using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NewFabrikAssets
{
    interface IFabrikN
    {
        Vector3 Root { get; set; }
        //Vector3 Target { get; set; }
        void UpdatePositions();

        void ChangeRoot(Vector3 root);
        void IterateBackwards();
        void IterateForwards();
        void Run(int loops);
    }
}
