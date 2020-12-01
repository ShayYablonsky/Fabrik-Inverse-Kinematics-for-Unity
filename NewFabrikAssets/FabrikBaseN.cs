using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NewFabrikAssets
{
    class FabrikBaseN : FabrikSubBaseN
    {
        public int Iterations = 5;
        public bool MoveAble = false;
        private void Awake()
        {
            Moveable = MoveAble;
            base.Awake();
        }
        private void Update()
        {
            Run(Iterations);
            UpdatePositions();
        }
        private void Run(int loops = 1)
        {
            for (int i = 0; i < loops; i++)
            {
                foreach (var chain in Chains)
                {
                    chain.IterateBackwards();
                }
                IterateBackwards();
                IterateForwards();
                foreach (var chain in Chains)
                {
                    chain.IterateForwards();
                }
                if (Moveable == false)
                {
                    Root = transform.position;
                }
            }
        }
    }
}
