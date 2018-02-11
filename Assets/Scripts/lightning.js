// ######################################################################
//
// lightning.js
//
// by Paul Mikulecky (www.lostpencil.com)
//
// Description:
//
// Create a new lightning bolt every frame. If an object is within a certain distance
// (maxLightningHitDistance) then hit that object with the bolt, otherwise end the bolt
// in mid air at fade_out_y distance from the current object. Spread the end of the lightning bolts
// in around an area under the current object within rangeMin and rangeMax values. Each 
// bolt will have numVertices line segments defining the bolt. Deviate each
// segment of the bolt by maxDeviate amount.
//
// Usage:
//
// 1. Create a sphere
// 2. Add Lightning script as component to Sphere
// 3. Add Line Renderer component to Sphere
// 4. Create a plane
// 5. Position sphere above plane
// 6. Press play (tweak parameters)
//
// Limitations: 
//
// No control over number of lightning bolts.
// Bolts are created every fixed frame
// Bolts don't follow start/end point when source object moves.
//
// ######################################################################

var lightningWidth = 0.05;				// line renderer line width
var maxLightningHitDistance = 500.0;	// how far to check for objects when determining end point of lightning bolt
var numVertices = 10;					// number of line segments per bolt
var rangeMin = -5.0;					// minimum range of end point of bolt
var rangeMax = 5.0;						// maximum range of end point of bolt
var maxDeviate = 2.0;					// maximum deviation amount of each line segment in bolt
var fade_out_y = 5.0;					// distance from object to end bolt if no object found to hit

function FixedUpdate() {

	// determine where the surface of the object is 
	// (1/2 the scale plus a bit so the raycast collider doesn't hit it)
	var sizeOfUnitSphere = (transform.localScale.y / 2.0) + .1; 
	var lr = GetComponent(LineRenderer);
	
	lr.SetWidth (lightningWidth, lightningWidth);
	lr.SetVertexCount (numVertices);

	// find start point on the surface of a 'unit' sphere
	var start_point = Random.onUnitSphere;

	start_point.x *= sizeOfUnitSphere;
	start_point.y *= sizeOfUnitSphere;
	start_point.z *= sizeOfUnitSphere;

	// limit random point to lower half of the unit sphere
	start_point.y = -Mathf.Abs(start_point.y);

	// add the object position to unit sphere
	start_point += transform.position;

	// get a random direction for the RayCast	
	var rndPoint = 
		Vector3.Normalize(
			Vector3(start_point.x + Random.Range(rangeMin, rangeMax), 
					start_point.y - maxLightningHitDistance, 
					start_point.z + Random.Range(rangeMin, rangeMax))
		);
	
	// find end point by casting a ray toward rndPoint from start point
	// if nothing around or farther than maxLightningHitDistance
	// then end lightning at fade_out_y distance
	//
	var hit : RaycastHit;
	var dist = 0.0;

	if (Physics.Raycast(start_point, rndPoint, hit, maxLightningHitDistance)) {
		dist = transform.position.y - hit.point.y;
		end_point = hit.point;
	} else {
		dist = transform.position.y - fade_out_y;
		end_point = Vector3(0.0, dist, 0.0);
	}

	// limit and randomize the distance in x and z of the final end_point 
	end_point.x = Random.Range(start_point.x + rangeMin, start_point.x + rangeMax);
	end_point.z = Random.Range(start_point.z + rangeMin, start_point.z + rangeMax);	
	
	// attenuate the vertical deviation by the height of the object above ground
	var tempDeviateY = 0.0;
	tempDeviateY = (dist / numVertices) * 2.0;

	// fill the lightning linerenderer with appropriate points to draw the strike
	var temp_start = start_point;
	var temp_end = Vector3(0.0, 0.0, 0.0);

	var temp_bias_x = (end_point.x - start_point.x) / numVertices;
	var temp_bias_z = (end_point.z - start_point.z) / numVertices;	
	var temp_bias_y = (end_point.y - start_point.y) / numVertices;

	lr.SetPosition(0, start_point);
	for (var i=1; i<numVertices; i++) {

		// calculate temp end point while biasing toward end_point
		temp_end.y = temp_start.y + temp_bias_y;
		temp_end.x = Random.Range(temp_start.x-maxDeviate+temp_bias_x, temp_start.x+maxDeviate+temp_bias_x);			
		temp_end.z = Random.Range(temp_start.z-maxDeviate+temp_bias_z, temp_start.z+maxDeviate+temp_bias_z);			

		lr.SetPosition(i, temp_end);
		temp_start = temp_end;
	}
	lr.SetPosition(numVertices-1, end_point);
}