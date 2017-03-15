public enum InfoType{
	None,
	Food,
	Customer
}

public enum StartMenuEntrances {
	DinerEntrance,
	DecoEntrance,
	ChallengeEntrance
}

// Different types of assets that you can unlock in each tier
public enum AssetTypes{
	Customer,
	DecoSpecial,
	DecoBasic,
	Food,
	Slot,
	Challenge,
	StarPiece
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
	Playful,
	HospitalRunner,
	Gossiper,
	CoolKid,
	BlackOut,
}

public enum PowerUpTypes{
	None,
}

// The TYPE of decorations available, not to be confused with deco shop Tab types
public enum DecoTypes{
	None,
	Table,
	Floor,
	Kitchen,
	PlayArea,
	VIP,
	FlyThru,
	Microwave,
	Special
}

// Where each deco should go in the deco shop tabs, NOT always the same as DecoTypes
public enum DecoTabTypes {
	None,
	Table,
	Floor,
	Kitchen,
	PlayArea,
	VIP,
	FlyThru,
	Microwave,
	Random,
	IAP
}

public enum PropNodeType{
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

public enum Objectives {
	None,
	Cash,
	MissedCustomers,
	WheatServed,
	DairyServed,
	PeanutServed,
	AllergyAttack,
}

public enum DifficultyLevels {
	None,
	Easy,
	Medium,
	Hard,
}

public enum MissionType {
	Customer,
	Food,
	Days,
	Walk,
	Allergy,
}

public enum AdConversionType {
	PageClick,		// Has clicked anywhere in the page
	ButtonClick		// Has clicked the link button
}