using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoFixture.Xunit2;
using Lagkage.Contracts.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Lagkage.Integration.Tests.AcceptanceTests;

[Collection("Http collection")]
public class CakeLayerTests
{
    HttpFixture _fixture;

    public CakeLayerTests(HttpFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory, AutoData]
    public async Task POST_CakeLayer_adds_cakelayer(
        AddCakeLayerRequest addCakeLayerRequest)
    {
        // Act
        var response = await _fixture.client.PostAsJsonAsync("minimalapis/api/cakelayers",addCakeLayerRequest );

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseBody = JsonSerializer.Deserialize<AddCakeLayerResponse>(responseString, _fixture.serializerOptions);
        Assert.NotNull(responseBody);
        Assert.True(Guid.TryParse(responseBody.CakeLayerId, out _));
    }
    
    [Theory, AutoData]
    public async Task GET_CakeLayer_By_Id_After_Posting(
        AddCakeLayerRequest addCakeLayerRequest)
    {
        // Arrange
        var postResponse = await _fixture.client.PostAsJsonAsync("minimalapis/api/cakelayers",addCakeLayerRequest );
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        
        // Act
        var getResponse = await _fixture.client.GetAsync(postResponse.Headers.Location);

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var getResponseString = await getResponse.Content.ReadAsStringAsync();
        var getResponseBody = JsonSerializer.Deserialize<HttpCakeLayer>(getResponseString, _fixture.serializerOptions);
        Assert.NotNull(getResponseBody);
        Assert.Equal(addCakeLayerRequest.HttpCakeLayer.Name, getResponseBody.Name);
    }
    
    [Theory, AutoData]
    public async Task DELETE_CakeLayer_By_Id_After_Posting(
        AddCakeLayerRequest addCakeLayerRequest)
    {
        // Arrange
        var postResponse = await _fixture.client.PostAsJsonAsync("minimalapis/api/cakelayers",addCakeLayerRequest );
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        
        // Act
        var deleteResponse = await _fixture.client.DeleteAsync(postResponse.Headers.Location);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        var getResponse = await _fixture.client.GetAsync(postResponse.Headers.Location);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Theory, AutoData]
    public async Task GET_CakeLayers_Returns_Non_Empty_List_After_Posting(
        AddCakeLayerRequest addCakeLayerRequest)
    {
        // Arrange
        var postResponse = await _fixture.client.PostAsJsonAsync("minimalapis/api/cakelayers",addCakeLayerRequest );
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        
        // Act
        var getResponse = await _fixture.client.GetAsync("minimalapis/api/cakelayers");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var getResponseString = await getResponse.Content.ReadAsStringAsync();
        var getResponseBody = JsonSerializer.Deserialize<GetCakeLayersResponse>(getResponseString, _fixture.serializerOptions);
        Assert.NotNull(getResponseBody);
        Assert.NotEmpty(getResponseBody.CakeLayers);
    }
}