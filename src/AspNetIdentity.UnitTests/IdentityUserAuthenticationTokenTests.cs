using FluentAssertions;
using Microsoft.AspNetCore.Identity.MongoDB;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AspNetIdentity.UnitTests
{
    public class IdentityUserAuthenticationTokenTests
    {
        [Fact]
        public void GetToken_NoTokens_ReturnsNull()
        {
            var user = new IdentityUser();

            var value = user.GetTokenValue("loginProvider", "tokenName");

            value.Should().BeNull();
        }

        [Fact]
        public void GetToken_WithToken_ReturnsValueIfProviderAndNameMatch()
        {
            var user = new IdentityUser();
            user.SetToken("loginProvider", "tokenName", "tokenValue");

            user.GetTokenValue("loginProvider", "tokenName").Should().Be("tokenValue", "GetToken should match on both provider and name, but isn't");

            user.GetTokenValue("wrongProvider", "tokenName").Should().BeNull("GetToken should match on loginProvider, but isn't");

            user.GetTokenValue("wrongProvider", "tokenName").Should().BeNull("GetToken should match on tokenName, but isn't");
        }

        [Fact]
        public void RemoveToken_OnlyRemovesIfNameAndProviderMatch()
        {
            var user = new IdentityUser();
            user.SetToken("loginProvider", "tokenName", "tokenValue");

            user.RemoveToken("wrongProvider", "tokenName");
            user.GetTokenValue("loginProvider", "tokenName").Should().Be("tokenValue", "RemoveToken should match on loginProvider, but isn't");

            user.RemoveToken("loginProvider", "wrongName");
            user.GetTokenValue("loginProvider", "tokenName").Should().Be("tokenValue", "RemoveToken should match on tokenName, but isn't");

            user.RemoveToken("loginProvider", "tokenName");
            user.GetTokenValue("loginProvider", "tokenName").Should().BeNull("RemoveToken should match on both loginProvider and tokenName, but isn't");
        }

        [Fact]
        public void SetToken_ReplacesValue()
        {
            var user = new IdentityUser();
            user.SetToken("loginProvider", "tokenName", "tokenValue");

            user.SetToken("loginProvider", "tokenName", "updatedValue");

            user.Tokens.Count.Should().Be(1);

            user.GetTokenValue("loginProvider", "tokenName").Should().Be("updatedValue");;
        }
    }
}
