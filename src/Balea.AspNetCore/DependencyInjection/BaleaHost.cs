using System.Collections.Generic;

namespace Balea
{
	public class BaleaWebHost
    {
		public BaleaEvents Events { get; set; } = new();
		public List<string> Schemes { get; set; } = new();
	}
}
