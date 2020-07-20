namespace RaiseYourHand
{
	public class Hand
	{
		public enum EHandType
		{
			Technical = 0,
			Bullshit = 1,
			TooMuch = 2,
			Comment = 3,
			NewTopic = 4
		}

		public Participant Participant { get; }
		public EHandType HandType { get; }
		public string HandText =>
			GetText(this.HandType);

		public Hand(Participant participant, EHandType handType)
		{
			this.Participant = participant;
			this.HandType = handType;
		}
		public static string GetText(EHandType handType)
		{
			switch (handType)
			{
				case EHandType.Technical:
				case EHandType.Bullshit:
				case EHandType.Comment:
					return handType.ToString();
				case EHandType.TooMuch:
					return "Too much...";
				case EHandType.NewTopic:
					return "New topic";
				default:
					return null;
			}
		}
	}
}