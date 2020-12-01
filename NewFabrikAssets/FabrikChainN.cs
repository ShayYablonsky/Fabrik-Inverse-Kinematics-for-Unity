using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NewFabrikAssets
{
    class FabrikChainN : IFabrikN
    {
        public Vector3 Root { get; set; }
        public IFabrikN Extention;
        public Transform[] Transforms;
        public Vector3[] Chain;
        private float[] Distance;
        public Vector3 Target { set; get; }
        public Transform TargetTransform;

        public FabrikChainN(Transform transform)
        {
            CreateChain(transform);
            Initialize();
            UpdateDistances();
        }
        public FabrikChainN(Transform start, Transform subbase)
        {
            // should be called from the subbase class only
            CreateChain(start);
            Chain = new Vector3[Transforms.Length + 1];
            Chain[0] = subbase.position;
            for (int i = 1; i < Chain.Length; i++)
            {
                Chain[i] = Transforms[i - 1].position; // -1 because the subbase is not in the transforms array
            }
            Initialize();
            UpdateDistances();
            Root = Chain[0];
        }
        private void CreateChain(Transform transform)
        {
            Stack<Transform> transforms = new Stack<Transform>();
            transforms.Push(transform);
            CreateChainRecursive(transforms);
            Transforms = transforms.ToArray();
            Reverse.ReverseArray(Transforms);
            Chain = new Vector3[Transforms.Length];
            for (int i = 0; i < Transforms.Length; i++)
            {
                Chain[i] = Transforms[i].position;
            }
        }
        private void CreateChainRecursive(Stack<Transform> stack)
        {
            switch (stack.Peek().childCount)
            {
                case 0: // if none end chain
                    return;
                case 1: // if only one child add to chain
                    stack.Push(stack.Peek().GetChild(0));
                    CreateChainRecursive(stack);
                    break;
                default: // if more than 1 child create subBase
                    Extention = stack.Peek().gameObject.AddComponent<FabrikSubBaseN>(); // replace with new subbase later
                    break;
            }
        }
        public void ChangeRoot(Vector3 root)
        {
            throw new NotImplementedException();
        }

        public void IterateBackwards()
        {
            if (Extention != null)
            {
                Extention.IterateBackwards();
                Target = Extention.Root;
            }
            else
            {
                Target = TargetTransform.position;
            }
            if (Vector3.Distance(Root, Target) > Distance.Sum())
            {
                Debug.Log("Target out of reach for the chain!");
                Target -= CalculateLine(Root, Target) * (Vector3.Distance(Root,Target) - Distance.Sum());
            }
            Chain[Chain.Length - 1] = Target;

            for (int i = Chain.Length-2; i >= 0; i--)
            {
                float lamb = Distance[i] / Vector3.Distance(Chain[i + 1], Chain[i]);
                Chain[i] = (1 - lamb) * Chain[i + 1] + lamb * Chain[i];
            }
        }
        private Vector3 CalculateLine(Vector3 a, Vector3 b)
        {
            Vector3 temp = a - b;
            temp = temp.normalized;
            return new Vector3(Math.Abs(temp.x), Math.Abs(temp.y), Math.Abs(temp.z));
        }

        public void IterateForwards()
        {
            Chain[0] = Root;
            for (int i = 0; i < Chain.Length - 1; i++)
            {
                float lamb = Distance[i] / Vector3.Distance(Chain[i + 1], Chain[i]);
                Chain[i + 1] = (1 - lamb) * Chain[i] + lamb * Chain[i + 1]; // might be the source of bugs lambda is same for both functions
            }
        }

        public void Run(int loops)
        {
            if (Extention != null)
            {
                Extention.IterateBackwards(); // make sure to update centroid internally
                IterateBackwards();
                IterateForwards();
                Extention.IterateForwards();
            }
            else
            {
                IterateBackwards();
                IterateForwards();
            }
        }
        private void UpdateDistances()
        {
            for (int i = 0; i < Distance.Length; i++)
            {
                Distance[i] = Vector3.Distance(Chain[i], Chain[i + 1]);
            }
        }
        private void Initialize()
        {
            if (Distance == null)
            {
                Distance = new float[Transforms.Length];
            }
        }
        public void UpdatePositions()
        {
            for (int i = 0; i < Transforms.Length; i++)
            {
                Transforms[i].position = Chain[i + 1];
            }
            if (Extention != null)
            {
                Extention.UpdatePositions();
            }
        }
    }
}
