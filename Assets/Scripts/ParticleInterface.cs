using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ParticleInterface
{
	ParticleSystem ThisObjectParticle {get;}

	void ActivateParticle(ParticleSystem ObjectParticle);	
}
