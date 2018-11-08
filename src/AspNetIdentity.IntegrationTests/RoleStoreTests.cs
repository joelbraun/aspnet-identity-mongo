using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace AspNetIdentity.IntegrationTests
{
	public class RoleStoreTests : UserIntegrationTestsBase
	{
		[Fact]
		public async Task Create_NewRole_Saves()
		{
			var roleName = "admin";
			var role = new IdentityRole(roleName);
			var manager = GetRoleManager();

			await manager.CreateAsync(role);

            var savedRole = Roles.Find(_ => true).Single();
            savedRole.Name.Should().Be(roleName);
            savedRole.NormalizedName.Should().Be("ADMIN");
		}

		[Fact]
		public async Task FindByName_SavedRole_ReturnsRole()
		{
			var roleName = "name";
			var role = new IdentityRole {Name = roleName};
			var manager = GetRoleManager();
			await manager.CreateAsync(role);

			// note: also tests normalization as FindByName now uses normalization
			var foundRole = await manager.FindByNameAsync(roleName);

            foundRole.Should().NotBeNull();
            foundRole.Name.Should().Be(roleName);
		}

		[Fact]
		public async Task FindById_SavedRole_ReturnsRole()
		{
			var roleId = ObjectId.GenerateNewId().ToString();
			var role = new IdentityRole {Name = "name"};
			role.Id = roleId;
			var manager = GetRoleManager();
			await manager.CreateAsync(role);

			var foundRole = await manager.FindByIdAsync(roleId);

            foundRole.Should().NotBeNull();
            foundRole.Id.Should().Be(roleId);
		}

		[Fact]
		public async Task Delete_ExistingRole_Removes()
		{
			var role = new IdentityRole {Name = "name"};
			var manager = GetRoleManager();
			await manager.CreateAsync(role);
            Roles.Find(_ => true).ToList().Should().NotBeEmpty();

			await manager.DeleteAsync(role);

            Roles.Find(_ => true).ToList().Should().BeEmpty();
		}

		[Fact]
		public async Task Update_ExistingRole_Updates()
		{
			var role = new IdentityRole {Name = "name"};
			var manager = GetRoleManager();
			await manager.CreateAsync(role);
			var savedRole = await manager.FindByIdAsync(role.Id);
			savedRole.Name = "newname";

			await manager.UpdateAsync(savedRole);

            var changedRole = Roles.Find(_ => true).Single();
            changedRole.Should().NotBeNull();
            changedRole.Name.Should().Be("newname");
		}

		[Fact]
		public async Task SimpleAccessorsAndGetters()
		{
			var role = new IdentityRole
			{
				Name = "name"
			};
			var manager = GetRoleManager();
			await manager.CreateAsync(role);

            manager.GetRoleIdAsync(role).Result.Should().Be(role.Id);
            manager.GetRoleNameAsync(role).Result.Should().Be("name");

			await manager.SetRoleNameAsync(role, "newName");
            manager.GetRoleNameAsync(role).Result.Should().Be("newName");
		}
	}
}