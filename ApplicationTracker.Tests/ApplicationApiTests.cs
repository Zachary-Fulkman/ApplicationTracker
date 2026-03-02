using System.Net;
using System.Net.Http.Json;
using ApplicationTracker.Dtos;
using ApplicationTracker.Models;
using Xunit;

namespace ApplicationTracker.Tests
{
    /// <summary>
    /// End-to-end integration tests for the Applications API.
    /// These tests spin up the real API with an in-memory database
    /// Verifies behavior through actual HTTP requests.
    /// </summary>
    public class ApplicationApiTests
    {
        // Creates a fresh API instance with its own in-memory database.
        // Each test gets a clean environment to avoid cross-test interference.
        private static HttpClient CreateClient()
        {
            var factory = new CustomWebApplicationFactory();
            return factory.CreateClient();
        }

        [Fact]
        public async Task Post_Creates_Application_And_Returns_201()
        {
            using var client = CreateClient();

            // Builds a valid create request
            var request = new CreateApplicationRequest
            {
                CompanyName = "TestCo",
                DateApplied = new DateOnly(2026, 3, 1),
                Status = "Applied",
                Notes = "First test"
            };

            // Sends POST request
            var response = await client.PostAsJsonAsync("/api/Application", request);

            // Verifies correct status code and returned data
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var created = await response.Content.ReadFromJsonAsync<ApplicationModel>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);
            Assert.Equal("TestCo", created.CompanyName);
        }

        [Fact]
        public async Task GetById_Returns_Application_When_It_Exists()
        {
            using var client = CreateClient();

            // Creates an application to retrieve
            var create = new CreateApplicationRequest
            {
                CompanyName = "GetByIdCo",
                DateApplied = new DateOnly(2026, 3, 1),
                Status = "Applied",
                Notes = null
            };

            var post = await client.PostAsJsonAsync("/api/Application", create);
            var created = await post.Content.ReadFromJsonAsync<ApplicationModel>();
            Assert.NotNull(created);

            // Fetch the newly created application by id
            var get = await client.GetAsync($"/api/Application/{created!.Id}");

            Assert.Equal(HttpStatusCode.OK, get.StatusCode);
            var fetched = await get.Content.ReadFromJsonAsync<ApplicationModel>();
            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched!.Id);
            Assert.Equal("GetByIdCo", fetched.CompanyName);
        }

        [Fact]
        public async Task Search_Returns_PagedResult_With_TotalCount()
        {
            using var client = CreateClient();

            // Add two records that match the same filter
            await client.PostAsJsonAsync("/api/Application", new CreateApplicationRequest
            {
                CompanyName = "Alpha",
                DateApplied = new DateOnly(2026, 3, 1),
                Status = "Interview",
                Notes = null
            });

            await client.PostAsJsonAsync("/api/Application", new CreateApplicationRequest
            {
                CompanyName = "Beta",
                DateApplied = new DateOnly(2026, 3, 1),
                Status = "Interview",
                Notes = null
            });

            // Request only 1 result per page
            var response = await client.GetAsync("/api/Application?status=Interview&page=1&pageSize=1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<PagedResult<ApplicationModel>>();

            // Even though only 1 item is returned, totalCount should reflect all matches
            Assert.NotNull(result);
            Assert.Equal(2, result!.TotalCount);
            Assert.Equal(1, result.Page);
            Assert.Equal(1, result.PageSize);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task Put_Updates_Application_When_It_Exists()
        {
            using var client = CreateClient();

            // Create a record to update
            var post = await client.PostAsJsonAsync("/api/Application", new CreateApplicationRequest
            {
                CompanyName = "BeforeUpdate",
                DateApplied = new DateOnly(2026, 3, 1),
                Status = "Applied",
                Notes = null
            });

            var created = await post.Content.ReadFromJsonAsync<ApplicationModel>();
            Assert.NotNull(created);

            // Update the record
            var updateRequest = new UpdateApplicationRequest
            {
                CompanyName = "AfterUpdate",
                DateApplied = new DateOnly(2026, 3, 2),
                Status = "Interview",
                Notes = "Updated"
            };

            var put = await client.PutAsJsonAsync($"/api/Application/{created!.Id}", updateRequest);
            Assert.Equal(HttpStatusCode.NoContent, put.StatusCode);

            // Verify changes persisted
            var get = await client.GetAsync($"/api/Application/{created.Id}");
            var updated = await get.Content.ReadFromJsonAsync<ApplicationModel>();
            Assert.NotNull(updated);
            Assert.Equal("AfterUpdate", updated!.CompanyName);
            Assert.Equal(new DateOnly(2026, 3, 2), updated.DateApplied);
            Assert.Equal("Interview", updated.Status);
            Assert.Equal("Updated", updated.Notes);
        }

        [Fact]
        public async Task Delete_Removes_Application_When_It_Exists()
        {
            using var client = CreateClient();

            // Create a record to delete
            var post = await client.PostAsJsonAsync("/api/Application", new CreateApplicationRequest
            {
                CompanyName = "DeleteMe",
                DateApplied = new DateOnly(2026, 3, 1),
                Status = "Applied",
                Notes = null
            });

            var created = await post.Content.ReadFromJsonAsync<ApplicationModel>();
            Assert.NotNull(created);

            // Delete it
            var del = await client.DeleteAsync($"/api/Application/{created!.Id}");
            Assert.Equal(HttpStatusCode.NoContent, del.StatusCode);

            // Verify Delete
            var get = await client.GetAsync($"/api/Application/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        }
    }
}
