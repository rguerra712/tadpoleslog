using System;
using FluentAssertions;
using NUnit.Framework;
using TadpolesLog.Inputs;

namespace TadpolesLog.Tests.Inputs
{
    [TestFixture]
    public class LoginCredentialsTest
    {
        [TestCase(null, typeof (ArgumentNullException))]
        [TestCase("", typeof (ArgumentException))]
        [TestCase("-username", typeof (ArgumentException))]
        [TestCase("-password", typeof (ArgumentException))]
        [TestCase("-nothing", typeof (ArgumentException))]
        [TestCase("word", typeof (ArgumentException))]
        public void Should_throw_exception_given_invalid_arguments(string input, Type expectedExceptionType)
        {
            // Arrange
            var args = input?.Split(' ');

            // Act
            TestDelegate runScenario= () => LoginCredentials.ExtractFrom(args);

            // Assert
            Assert.Throws(expectedExceptionType, runScenario);
        }

        [TestCase("/username user /password pass", "user", "pass")]
        public void Should_be_able_to_extract_username_and_password_in_happy_paths(
            string input,
            string expectedUserName,
            string expectedPassword)
        {
            // Arrange
            var args = input.Split(' ');

            // Act
            var loginCredentials = LoginCredentials.ExtractFrom(args);

            // Assert
            loginCredentials.UserName.Should().Be(expectedUserName);
            loginCredentials.Password.Should().Be(expectedPassword);
        }
    }
}