using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Waes.Diffly.Api;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Api.Dtos.Enums;
using Waes.Diffly.IntegrationTest.Extension;
using Xunit;

namespace Waes.Diffly.IntegrationTest
{
    public class DiffControllerTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        private readonly string _apiBase = "/v1/diff/";
        private readonly Func<int, string> _apiLeftFactory;
        private readonly Func<int, string> _apiRightFactory;
        private readonly Func<int, string> _apiDiffFactory;

        public DiffControllerTest()
        {
            _apiLeftFactory = new Func<int, string>(id => $"{_apiBase}{id}/left");
            _apiRightFactory = new Func<int, string>(id => $"{_apiBase}{id}/right");
            _apiDiffFactory = new Func<int, string>(id => $"{_apiBase}{id}");

            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());

            _client = _server.CreateClient();
        }

        /// <summary>
        /// Tests positive scenrios - sucessful diffs, with all possible results.
        /// </summary>
        /// <param name="encodedDataLeft">Left Base64 binary data.</param>
        /// <param name="encodedDataRight">Right Base64 binary data.</param>
        /// <param name="expectedResult">Result expected from API.</param>
        /// <returns></returns>
        [Theory]
        [InlineData("VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==", "VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==", DiffResultType.Equal)]
        [InlineData("VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==", "QUJDRERB", DiffResultType.NotEqualSize)]
        [InlineData("VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==", "VGhpcyBpcyB3b3JraW5nIGdyZWF0Lg==", DiffResultType.NotEqual)]
        public async Task PutToLeftAndPutToRightAndDiff_Returns200InAllCases_DiffResultAsExpected(
            string encodedDataLeft, string encodedDataRight, DiffResultType expectedResult)
        {
            // Arrange
            int id = 1;
            var requestDto1 = new DiffRequestDto() { EncodedData = encodedDataLeft};
            var requestDto2 = new DiffRequestDto() { EncodedData = encodedDataRight};

            // Act
            var response1 = await _client.PutAsync(_apiLeftFactory(id), requestDto1.ToJsonHttpContent());
            response1.EnsureSuccessStatusCode();

            var response2 = await _client.PutAsync(_apiRightFactory(id), requestDto2.ToJsonHttpContent());
            response2.EnsureSuccessStatusCode();

            var responseDiff = await _client.GetAsync(_apiDiffFactory(id));
            responseDiff.EnsureSuccessStatusCode();

            var responseString = await responseDiff.Content.ReadAsStringAsync();
            var resultDto = JsonConvert.DeserializeObject<DiffResultDto>(responseString);
            
            // Assert
            Assert.Equal(resultDto.Result, expectedResult);
        }

        [Fact]
        public async Task DoPostRequestTwiceForSameIdAndSide_Returns400AndErrorDto()
        {
            // Arrange
            int id = 2;
            var requestDto = new DiffRequestDto() { EncodedData = "VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==" };
            string leftUri = _apiLeftFactory(id);

            // Act
            var response1 = await _client.PostAsync(leftUri, requestDto.ToJsonHttpContent());
            response1.EnsureSuccessStatusCode();

            var response2 = await _client.PostAsync(leftUri, requestDto.ToJsonHttpContent());
            var errorDto = await response2.ToDto<ErrorDto>();

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
            Assert.Equal(errorDto.Message, "Left value already set! Maybe you want to update it using PUT?");
        }

        [Fact]
        public async Task DiffSideRouteConstraint_WhenRouteIsNotLeftOrRight_404Returned()
        {
            int id = 3;
            var requestDto = new DiffRequestDto() { EncodedData = "VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==" };
            string uri = $"{_apiBase}{id}/something";

            var response = await _client.PostAsync(uri, requestDto.ToJsonHttpContent());

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
