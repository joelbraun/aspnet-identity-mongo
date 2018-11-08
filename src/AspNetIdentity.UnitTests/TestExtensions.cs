namespace Tests
{
	using System.Linq;
	using System.Security.Claims;
    using FluentAssertions;
    using Microsoft.AspNetCore.Identity.MongoDB;

	public static class TestExtensions
	{
		public static void ExpectOnlyHasThisClaim(this IdentityUser user, Claim expectedClaim)
		{

            user.Claims.Count.Should().Be(1);

			var actualClaim = user.Claims.Single();
            actualClaim.Type.Should().Be(expectedClaim.Type);
            actualClaim.Value.Should().Be(expectedClaim.Value);
		}
	}
}