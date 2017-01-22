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
        [InlineData("VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==", "VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==", DiffResultType.Equal, 11)]
        [InlineData("VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==", "QUJDRERB", DiffResultType.SizeDoNotMatch, 12)]
        [InlineData("VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==", "VGhpcyBpcyB3b3JraW5nIGdyZWF0Lg==", DiffResultType.ContentDoNotMatch, 13)]
        public async Task PutToLeftAndPutToRightAndDiff_Returns200InAllCases_DiffResultAsExpected(
            string encodedDataLeft, string encodedDataRight, DiffResultType expectedResult, int id)
        {
            // Arrange
            var requestDto1 = new DiffRequestDto(encodedDataLeft);
            var requestDto2 = new DiffRequestDto(encodedDataRight);

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
            int id = 20;
            var requestDto = new DiffRequestDto("VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==");
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
            int id = 30;
            var requestDto = new DiffRequestDto("VGhpcyBpcyB3b3JraW5nIGdyZWF0IQ==");
            string uri = $"{_apiBase}{id}/something";

            var response = await _client.PostAsync(uri, requestDto.ToJsonHttpContent());

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdatingOfCreatedEntity_ChangesDiff()
        {
            // Arrange
            int id = 40;
            var requestDto1 = new DiffRequestDto("RXF1YWw="); //"Equal"
            var requestDto2 = new DiffRequestDto("RXF1YWw=");

            // Act - submit left, right & do diff
            var response1 = await _client.PutAsync(_apiLeftFactory(id), requestDto1.ToJsonHttpContent());
            response1.EnsureSuccessStatusCode();

            var response2 = await _client.PutAsync(_apiRightFactory(id), requestDto2.ToJsonHttpContent());
            response2.EnsureSuccessStatusCode();

            var responseDiff = await _client.GetAsync(_apiDiffFactory(id));
            responseDiff.EnsureSuccessStatusCode();
            var responseString = await responseDiff.Content.ReadAsStringAsync();
            var resultDto = JsonConvert.DeserializeObject<DiffResultDto>(responseString);

            Assert.Equal(resultDto.Result, DiffResultType.Equal); // Test result of the 1st Diff

            // Act - update right value & do diff
            requestDto2 = new DiffRequestDto("Tm90RXF1YWw="); //"NotEqual"
            var response4 = await _client.PutAsync(_apiRightFactory(id), requestDto2.ToJsonHttpContent());
            response4.EnsureSuccessStatusCode();

            responseDiff = await _client.GetAsync(_apiDiffFactory(id));
            responseDiff.EnsureSuccessStatusCode();
            responseString = await responseDiff.Content.ReadAsStringAsync();
            resultDto = JsonConvert.DeserializeObject<DiffResultDto>(responseString);

            // Assert
            Assert.Equal(resultDto.Result, DiffResultType.SizeDoNotMatch); // Test result of the 2st Diff
        }


        /*
        1	GET /v1/diff/1	404 Not Found
        2	PUT /v1/diff/1/left   "data": "AAAAAA==" - 	201 Created
        3	GET /v1/diff/1	404 Not Found
        4	PUT /v1/diff/1/right   "data": "AAAAAA==" - 	201 Created
        5	GET /v1/diff/1	200 OK   "diffResultType": "Equals" - 
        6	PUT /v1/diff/1/right   "data": "AQABAQ==" - 	201 Created
        7	GET /v1/diff/1	200 OK   "diffResultType": "ContentDoNotMatch",
          "diffs": [
                  "offset": 0,
              "length": 1
            },
                  "offset": 2,
              "length": 2
            }
          ] - 
        8	PUT /v1/diff/1/left    "data": "AAA=" - 	201 Created
        9	GET /v1/diff/1	200 OK   "diffResultType": "SizeDoNotMatch" - 
        10	PUT /v1/diff/1/left    "data": null - 	400 Bad Request
        */
        [Fact]
        public async Task AssignmentSampleInputOutput_Succeeds()
        {
            int id = 1;
            string uriDiff = _apiDiffFactory(id);
            string uriLeft = _apiLeftFactory(id);
            string uriRight = _apiRightFactory(id);
            var zerosRequest = new DiffRequestDto("AAAAAA==");

            var response1 = await _client.GetAsync(uriDiff);
            Assert.Equal(HttpStatusCode.NotFound, response1.StatusCode);

            var response2 = await _client.PutAsync(uriLeft, zerosRequest.ToJsonHttpContent());
            Assert.Equal(HttpStatusCode.Created, response2.StatusCode);

            var response3 = await _client.GetAsync(uriDiff);
            Assert.Equal(HttpStatusCode.NotFound, response3.StatusCode);

            var response4 = await _client.PutAsync(uriRight, zerosRequest.ToJsonHttpContent());
            Assert.Equal(HttpStatusCode.Created, response4.StatusCode);

            var response5 = await _client.GetAsync(uriDiff);
            Assert.Equal(HttpStatusCode.OK, response5.StatusCode);
            var dto5 = await response5.ToDto<DiffResultDto>();
            Assert.Equal(DiffResultType.Equal, dto5.Result);

            var response6 = await _client.PutAsync(uriRight, new DiffRequestDto("AQABAQ==").ToJsonHttpContent());
            Assert.Equal(HttpStatusCode.Created, response6.StatusCode);

            var response7 = await _client.GetAsync(uriDiff);
            Assert.Equal(HttpStatusCode.OK, response7.StatusCode);
            var dto7 = await response7.ToDto<DiffResultDto>();
            Assert.Equal(DiffResultType.ContentDoNotMatch, dto7.Result);
        }

    }
}
