namespace Balea
{
    public class BaleaOptions
    {
		public string ApplicationName { get; set; } = BaleaConstants.DefaultApplicationName;
		public ClaimTypeMap ClaimTypeMap { get; set; } = new ClaimTypeMap();
    }
}
