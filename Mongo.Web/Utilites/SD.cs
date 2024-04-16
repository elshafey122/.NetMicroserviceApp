namespace Mongo.Web.Utilites
{
	public class SD
	{
		public static string CouponApi { get; set; }
		public static string ProductApi { get; set; }
		public static string AuthApi { get; set; }
		public static string ShoppingCartApi { get; set; }
		public static string OrderApi { get; set; }

		public const string RoleAdmin = "ADMIN";
		public const string RoleCustomer = "CUSTOMER";
		public const string TokenCookie = "JWTToken";
		public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}

		public const string Status_Pending = "Pending";
		public const string Status_Approved = "Approved";
		public const string Status_ReadyForPickup = "ReadyForPickup";
		public const string Status_Completed = "Completed";
		public const string Status_Refunded = "Refunded";
		public const string Status_Cancelled = "Cancelled";

		public enum ContentType
		{
			Json,
			MultipartFormData,
		}
	}
}
