using UnityEngine;


namespace Prototyping.Games
{

    public enum BlockSizeType
    {
        Full = 0,
        Half = 1,
        One =  2
    }

    public class InfinityRunnerBlock : MonoBehaviour
    {
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        [SerializeField] private BlockSizeType blockSizeType;

        public Transform GetStart { get { return start; } }
        public Transform End { get { return end; } }

        private int lane = 0;

        // if these blocks become necesarry
        //public void SetSidePosition (float dashDistance)
        //{

        //    if (blockSizeType == BlockSizeType.Half)
        //    {
        //        float moveDash = dashDistance / 2f;
        //        int[] picks = new int[2] {-1, 1};
        //        lane = picks[Random.Range(0, 2)];
        //        float pos = lane * moveDash; 
        //        transform.position = new Vector3(transform.position.x + pos, transform.position.y, transform.position.z);
        //    }
        //    if (blockSizeType == BlockSizeType.One)
        //    {
        //        lane = Random.Range(-1, 2);
        //        float pos = lane * dashDistance;
        //        transform.position = new Vector3(transform.position.x + pos, transform.position.y, transform.position.z);
        //    }
        //}

    }
}

