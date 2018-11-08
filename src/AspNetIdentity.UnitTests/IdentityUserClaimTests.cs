using FluentAssertions;
using Microsoft.AspNetCore.Identity.MongoDB;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Tests;
using Xunit;

namespace AspNetIdentity.UnitTests
{
    public class IdentityUserClaimTests
    {
        [Fact]
        public void Create_FromClaim_SetsTypeAndValue()
        {
            var claim = new Claim("type", "value");

            var userClaim = new IdentityUserClaim(claim);

            userClaim.Type.Should().Be("type");
            userClaim.Value.Should().Be("value");
        }

        [Fact]
        public void ToSecurityClaim_SetsTypeAndValue()
        {
            var userClaim = new IdentityUserClaim { Type = "t", Value = "v" };

            var claim = userClaim.ToSecurityClaim();

            claim.Type.Should().Be("t");
            claim.Value.Should().Be("v");
        }

        [Fact]
        public void ReplaceClaim_NoExistingClaim_Ignores()
        {
            // note: per EF implemention - only existing claims are updated by looping through them so that impl ignores too
            var user = new IdentityUser();
            var newClaim = new Claim("newType", "newValue");

            user.ReplaceClaim(newClaim, newClaim);

            user.Claims.Should().BeEmpty();
        }

        [Fact]
        public void ReplaceClaim_ExistingClaim_Replaces()
        {
            var user = new IdentityUser();
            var firstClaim = new Claim("type", "value");
            user.AddClaim(firstClaim);
            var newClaim = new Claim("newType", "newValue");

            user.ReplaceClaim(firstClaim, newClaim);

            user.ExpectOnlyHasThisClaim(newClaim);
        }

        [Fact]
        public void ReplaceClaim_ValueMatchesButTypeDoesNot_DoesNotReplace()
        {
            var user = new IdentityUser();
            var firstClaim = new Claim("type", "sameValue");
            user.AddClaim(firstClaim);
            var newClaim = new Claim("newType", "sameValue");

            user.ReplaceClaim(new Claim("wrongType", "sameValue"), newClaim);

            user.ExpectOnlyHasThisClaim(firstClaim);
        }

        [Fact]
        public void ReplaceClaim_TypeMatchesButValueDoesNot_DoesNotReplace()
        {
            var user = new IdentityUser();
            var firstClaim = new Claim("sameType", "value");
            user.AddClaim(firstClaim);
            var newClaim = new Claim("sameType", "newValue");

            user.ReplaceClaim(new Claim("sameType", "wrongValue"), newClaim);

            user.ExpectOnlyHasThisClaim(firstClaim);
        }
    }
}
