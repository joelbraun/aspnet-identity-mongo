using AspNetIdentity.IntegrationTests;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;
using Xunit;

namespace AspNetIdentity.IntegrationTests
{

	public class IdentityUserTests : UserIntegrationTestsBase
	{
		[Fact]
		public void Insert_NoId_SetsId()
		{
			var user = new IdentityUser();
			user.Id = null;

			Users.InsertOne(user);

            user.Id.Should().NotBeNull();
			var parsed = user.Id.SafeParseObjectId();

            parsed.Should().NotBeNull();
            parsed.Should().NotBe(ObjectId.Empty);
		}
	}
}