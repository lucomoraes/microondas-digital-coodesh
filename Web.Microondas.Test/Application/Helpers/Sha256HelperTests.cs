using Web.Microondas.Application.Helpers;

namespace Web.Microondas.Test.Application.Helpers;

public class Sha256HelperTests
{
    [Fact]
    public void Hash_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = "admin";

        // Act
        var hash1 = Sha256Helper.Hash(input);
        var hash2 = Sha256Helper.Hash(input);

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void Hash_ShouldReturnExpectedHash()
    {
        // Arrange
        var input = "admin";
        var expectedHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";

        // Act
        var result = Sha256Helper.Hash(input);

        // Assert
        result.Should().Be(expectedHash);
    }

    [Theory]
    [InlineData("password123")]
    [InlineData("test")]
    [InlineData("MySecurePassword!@#")]
    public void Hash_ShouldReturn64CharacterHash(string input)
    {
        // Act
        var result = Sha256Helper.Hash(input);

        // Assert
        result.Should().HaveLength(64);
    }

    [Fact]
    public void Hash_DifferentInputs_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input1 = "password1";
        var input2 = "password2";

        // Act
        var hash1 = Sha256Helper.Hash(input1);
        var hash2 = Sha256Helper.Hash(input2);

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void Hash_EmptyString_ShouldReturnHash()
    {
        // Arrange
        var input = "";

        // Act
        var result = Sha256Helper.Hash(input);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveLength(64);
    }
}
