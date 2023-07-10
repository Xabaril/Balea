namespace Balea
{
	public class BaleaAspNetOptions
    {
        public BaleaOptions Common { get; set; } = new();
		public BaleaWebHost WebHost { get; set; } = new();
	}
}
