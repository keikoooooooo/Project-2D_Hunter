using System.Collections;
using UnityEngine;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		/// 

		public Transform target;
        [HideInInspector] public Vector3 targetRandom;
		public BoxCollider2D boxCollider;

		public LayerMask layerMask;

		public bool isTargetRandom;

		IAstarAI ai;

		void OnEnable ()
		{
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.

			if (ai != null) ai.onSearchPath += Update;

            ai.canMove = false;
            targetRandom = transform.position;
            isTargetRandom = true;
            StartCoroutine(PositionRandom());
        }

		void OnDisable () 
		{
			if (ai != null) ai.onSearchPath -= Update;
		}

        /// <summary>Updates the AI's destination every frame</summary>
        void Update () 
		{

			if (target != null && ai != null)
			{
				if (isTargetRandom)
				{
					
					ai.destination = targetRandom;
				}

				else // nếu không dùng vị trí random
				{
					ai.destination = target.position;
				}               
            }

		}


		float posX, posY;
		IEnumerator PositionRandom()
		{
			while (true)
			{
                if (boxCollider != null)
				{
					posX = Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x);
					posY = Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y);

                    var hit = Physics2D.OverlapCircle(new Vector3(posX, posY, 0), 1f, layerMask);
					if(hit == null)
					{
						if(isTargetRandom) ai.canMove = true;
                        targetRandom = new Vector3(posX, posY, 0);
                    }
                }

                yield return new WaitForSeconds(5f);
            }
        }


        private void OnDrawGizmos()
        {		
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(targetRandom, 1f);
        }

    }
}
