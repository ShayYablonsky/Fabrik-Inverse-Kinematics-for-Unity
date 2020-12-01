using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NewFabrikAssets
{
    class FabrikSubBaseN : MonoBehaviour, IFabrikN
    {
        public Vector3 Root { set; get; }
        protected FabrikChainN[] Chains;
        public bool Moveable = true;
        public Transform[] Targets;

        protected void Awake()
        {
            if (Targets == null)
            {
                Targets = new Transform[transform.childCount];
                Debug.Log(Targets.Length + "Targets");
            }
            Root = transform.position;
            Chains = new FabrikChainN[transform.childCount];
            for (int i = 0; i < Chains.Length; i++)
            {
                Chains[i] = new FabrikChainN(transform.GetChild(i), transform);
                if (Chains[i].TargetTransform == null)
                {
                    Debug.Log(i);
                    if (Targets[i] == null)
                    {
                        Targets[i] = Chains[i].Transforms[Chains[i].Transforms.Length-1];
                    }
                    Debug.Log(i);
                    Chains[i].TargetTransform = Targets[i];
                }
            }
        }
        private void Update()
        {
            for (int i = 0; i < Chains.Length; i++)
            {
                Chains[i].TargetTransform = Targets[i];
            }

        }
        public void ChangeRoot(Vector3 root)
        {
            throw new NotImplementedException();
        }

        public void IterateBackwards()
        {
            foreach (var chain in Chains)
            {
                chain.IterateBackwards();
            }
            if (Moveable)
            {
                Root = GetCentroid();
            }
        }
        private Vector3 GetCentroid()
        {
            Vector3 centroid = new Vector3(0, 0, 0);
            foreach (var chain in Chains)
            {
                centroid += chain.Chain[0];
            }
            return centroid / Chains.Length;
        }

        public void IterateForwards()
        {
            foreach (var chain in Chains)
            {
                chain.Root = Root;
                chain.IterateForwards();
            }
        }

        public void Run(int loops)
        {
            throw new NotImplementedException();
        }
        public void UpdatePositions()
        {
            transform.position = Root;
            foreach (var chain in Chains)
            {
                chain.UpdatePositions();
            }
        }
    }
}
