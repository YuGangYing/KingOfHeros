using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode{

	public APIModel apiModel;
	public UpdateModel updateModel;

	public GameMode(){
		apiModel = new APIModel ();
		updateModel = new UpdateModel ();
	}

}
