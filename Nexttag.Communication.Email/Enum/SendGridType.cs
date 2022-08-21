using System.ComponentModel;
using System.Reflection;

namespace  Nexttag.Communication.Email.Enum;

	public enum SendGridType
	{
		[Description("1ce137b6-cc3a-4412-a417-5ab9cb7de0d6")]
		ImportFileExcelAlert,

		[Description("f6bfd361-2032-425d-a90c-79aaf0c060d2")]
		ChangedAccreditation,

		[Description("d06d83bd-2f9f-4e43-af99-1893f59b0a73")]
		ReceivedVoucher,

		[Description("fb8bc04c-8696-44b2-afd2-194d0091a6a6")]
		NewObjective,

		[Description("641da737-728b-40b8-8abc-78b93a14b038")]
		WeekOffers,
        
        [Description("5809e237-d33e-49de-b4dc-4cfde4f127b6")]
        CanceledUser,

        [Description("d-bc7b1149b9574f9d90da778ccdd1eb72")]
        CentralizedOffers
    }

	public static class Extensions
	{
		public static string GetDescription(this SendGridType e)
		{
			var attribute =
				e.GetType()
					.GetTypeInfo()
					.GetMember(e.ToString())
					.FirstOrDefault(member => member.MemberType == MemberTypes.Field)
					.GetCustomAttributes(typeof(DescriptionAttribute), false)
					.SingleOrDefault()
					as DescriptionAttribute;

			return attribute?.Description ?? e.ToString();

		}
	}

