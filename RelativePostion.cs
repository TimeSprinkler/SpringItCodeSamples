using UnityEngine;
using System.Collections;

static public class RelativePostion {

	static public Vector3 positionRelativeTo(Transform child, Transform targetParent){

		Transform currentChild = child;
		Transform currentParent = child.parent;

		Vector3 relativePosition = new Vector3(0, 0, 0);

		while(currentParent != null){

			relativePosition.x += currentChild.localPosition.x;
			relativePosition.y += currentChild.localPosition.y;
			relativePosition.z += currentChild.localPosition.z;

			if(currentParent.name == targetParent.name){
				return relativePosition;
			}

			currentChild = currentParent;
			currentParent = currentChild.parent;
		}

		if(relativePosition.magnitude == 0) relativePosition = currentChild.localPosition;

		return relativePosition;

	}

	static public Vector3 positionRelativeToNothing(Transform child){return positionRelativeTo(child, null);}
}
