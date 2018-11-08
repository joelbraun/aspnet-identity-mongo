using FluentAssertions;
using Microsoft.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;
using Tests;
using Xunit;

namespace AspNetIdentity.UnitTests
{
    public class IdentityRoleTests
    {
        [Fact]
        public void ToBsonDocument_IdAssigned_MapsToBsonObjectId()
        {
            var role = new IdentityRole();

            var document = role.ToBsonDocument();

            document["_id"].Should().BeOfType<BsonObjectId>();
        }

        [Fact]
        public void Create_WithoutRoleName_HasIdAssigned()
        {
            var role = new IdentityRole();

            var parsed = role.Id.SafeParseObjectId();

            parsed.Should().NotBeNull();

        }

        [Fact]
        public void Create_WithRoleName_SetsName()
        {
            var name = "admin";

            var role = new IdentityRole(name);

            role.Name.Should().Be(name);
        }

        [Fact]
        public void Create_WithRoleName_SetsId()
        {
            var role = new IdentityRole("admin");

            var parsed = role.Id.SafeParseObjectId();

            parsed.Should().NotBeNull();
            parsed.Should().NotBe(ObjectId.Empty);
        }
    }
}
