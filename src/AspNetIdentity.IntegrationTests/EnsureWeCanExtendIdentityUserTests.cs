using System.Linq;
using System.Threading.Tasks;
using AspNetIdentity.IntegrationTests;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace AspNetIdentity.IntegrationTests
{

	public class EnsureWeCanExtendIdentityUserTests : UserIntegrationTestsBase
	{
		private UserManager<ExtendedIdentityUser> _Manager;
		private ExtendedIdentityUser _User;

		public class ExtendedIdentityUser : IdentityUser
		{
			public string ExtendedField { get; set; }
		}

		public EnsureWeCanExtendIdentityUserTests()
		{
			_Manager = CreateServiceProvider<ExtendedIdentityUser, IdentityRole>()
				.GetService<UserManager<ExtendedIdentityUser>>();
			_User = new ExtendedIdentityUser
			{
				UserName = "bob"
			};
		}

		[Fact]
		public async Task Create_ExtendedUserType_SavesExtraFields()
		{
			_User.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_User);

			var savedUser = Users.Find(_ => true).As<ExtendedIdentityUser>().Single();

            savedUser.ExtendedField.Should().Be("extendedField");
		}

		[Fact]
		public async Task Create_ExtendedUserType_ReadsExtraFields()
		{
			_User.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_User);

			var savedUser = await _Manager.FindByIdAsync(_User.Id);

            savedUser.ExtendedField.Should().Be("extendedField");

		}
	}
}