using FluentAssertions;
using Microsoft.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;
using Tests;
using Xunit;

namespace AspNetIdentity.UnitTests
{
    public class IdentityUserTests
    {
        [Fact]
        public void ToBsonDocument_IdAssigned_MapsToBsonObjectId()
        {
            var user = new IdentityUser();

            var document = user.ToBsonDocument();

            document["_id"].Should().BeOfType<BsonObjectId>();
        }

        [Fact]
        public void Create_NewIdentityUser_HasIdAssigned()
        {
            var user = new IdentityUser();

            var parsed = user.Id.SafeParseObjectId();

            parsed.Should().NotBeNull();
            parsed.Should().NotBe(ObjectId.Empty);
        }

        [Fact]
        public void Create_NoPassword_DoesNotSerializePasswordField()
        {
            // if a particular consuming application doesn't intend to use passwords, there's no reason to store a null entry except for padding concerns, if that is the case then the consumer may want to create a custom class map to serialize as desired.

            var user = new IdentityUser();

            var document = user.ToBsonDocument();

            document.Contains("PasswordHash").Should().BeFalse();
        }

        [Fact]
        public void Create_NullLists_DoesNotSerializeNullLists()
        {
            // serialized nulls can cause havoc in deserialization, overwriting the constructor's initial empty list 
            var user = new IdentityUser();
            user.Roles = null;
            user.Tokens = null;
            user.Logins = null;
            user.Claims = null;

            var document = user.ToBsonDocument();

            document.Contains("Roles").Should().BeFalse();
            document.Contains("Tokens").Should().BeFalse();
            document.Contains("Logins").Should().BeFalse();
            document.Contains("Claims").Should().BeFalse();
        }

        [Fact]
        public void Create_NewIdentityUser_ListsNotNull()
        {
            var user = new IdentityUser();

            user.Logins.Should().BeEmpty();
            user.Tokens.Should().BeEmpty();
            user.Roles.Should().BeEmpty();
            user.Claims.Should().BeEmpty();
        }
    }
}
