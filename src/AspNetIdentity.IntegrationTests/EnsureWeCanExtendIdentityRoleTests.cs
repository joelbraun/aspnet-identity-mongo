using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace AspNetIdentity.IntegrationTests
{

	public class EnsureWeCanExtendIdentityRoleTests : UserIntegrationTestsBase
	{
		private RoleManager<ExtendedIdentityRole> _Manager;
		private ExtendedIdentityRole _Role;

		public class ExtendedIdentityRole : IdentityRole
		{
			public string ExtendedField { get; set; }
		}

		public EnsureWeCanExtendIdentityRoleTests()
		{
			_Manager = CreateServiceProvider<IdentityUser, ExtendedIdentityRole>()
				.GetService<RoleManager<ExtendedIdentityRole>>();
			_Role = new ExtendedIdentityRole
			{
				Name = "admin"
			};
		}

		[Fact]
		public async Task Create_ExtendedRoleType_SavesExtraFields()
		{
			_Role.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_Role);

            var savedRole = Roles.Find(_ => true).As<ExtendedIdentityRole>().Single();

            savedRole.Should().Be("extendedField");
		}

		[Fact]
		public async Task Create_ExtendedRoleType_ReadsExtraFields()
		{
			_Role.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_Role);

			var savedRole = await _Manager.FindByIdAsync(_Role.Id);

            savedRole.ExtendedField.Should().Be("extendedField");
		}
	}
}