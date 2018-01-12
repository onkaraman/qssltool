namespace SSLLabsApiWrapper.Models.Response.EndpointSubModels
{
	public class List
	{
		public int id { get; set; }
		public string name { get; set; }
        public int cipherStrength { get; set; }
        public string kxType { get; set; }
        public int kxStrength { get; set; }
        public int dhBits { get; set; }
        public int dhP { get; set; }
        public int dhG { get; set; }
        public int dhYs { get; set; }
	}
}
