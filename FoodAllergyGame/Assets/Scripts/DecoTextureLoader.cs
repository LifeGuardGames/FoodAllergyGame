using UnityEngine;
using System.Collections;

public class DecoTextureLoader : DecoLoader {
	public SpriteRenderer floorSpriteRenderer;

	protected override void DecoInit(){
		isDebugEnableDeco = true;	// Always on
		debugDecoID = Constants.GetDebugConstant<string>("FloorID");
		decoType = DecoTypes.Floor;
	}

	// Overrided parent function, special case
	public override void LoadDeco(ImmutableDataDecoItem decoData){
		floorSpriteRenderer.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);
	}
}
