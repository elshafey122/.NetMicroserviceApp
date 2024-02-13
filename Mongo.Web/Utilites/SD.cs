namespace Mongo.Web.Utilites
{
    public class SD
    {
        public static string CouponApi { get; set; }
        public static string AuthApi { get; set; }
		public static string ProductApi { get; set; }
        public static string ShoppingCartApi { get; set; }

        public static string RoleAdmin = "Admin";

        public static string RoleCustomer = "Customer";

        public static string TokenCookie = "Jwttoken";


        public enum ApiType
        {
            Get,
            Post,
            Put,
            Delete
        }
    }
}
