public enum InfoType{
	None,
	Food,
	Customer
}

// Different types of assets that you can unlock in each tier
public enum AssetTypes{
	Customer,
	DecoSpecial,
	DecoBasic,
	Food,
	Slot,
	Challenge
}

public enum ChallengeTypes {
	Regular,
	Tutorial
}

public enum FoodKeywords{
	None,
	Keyword1,
	Keyword2,
//	Nut,
//	Drink,
//	Green,
//	Meat,
//	Bread,
//	Chocolate,
//	Dessert,
	Meal,
	Dessert,
	Drink,
}

public enum Allergies{
	None,
	Wheat,
	Peanut,
	Dairy,
}

public enum CustomerStates{
	None,
	InLine,
	ReadingMenu,
	WaitForOrder,
	WaitForFood,
	Eating,
	WaitForCheck,
	AllergyAttack,
	Saved,
	Eaten,
}

public enum WaiterHands{
	None,
	Order,
	Meal,
}

public enum CustomerTypes{
	None,
	Normal,
	Impatient,
	Eater,
	TableSmasher,
	AllergyAttack,
	HospitalRunner,
	Gossiper,
	CoolKid,	// TODO Add more here
}

public enum PowerUpTypes{
	None,
}

public enum DecoTypes{
	None,
	Table,
	Floor,
	Kitchen,
	Bathroom,
	PlayArea,
	VIP,
	FlyThru,
	Microwave,
}

public enum  PropNodeType{
	None,
	Event,
	Growth,
	Random
}

public enum ChallengeReward {
	None,
	Stone,
	Bronze,
	Silver,
	Gold,
}

public enum GossiperMode {
	None,
	TableSmasher,
	Eater,
	Blackout,
	All,
}

