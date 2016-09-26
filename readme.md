Waes.Diffly
===================

This is a simple diffing service for the byte arrays, implemented using ASP.NET Core and tested with xUnit and Moq.

How it works
----------
It exposes three endpoints. Two of them where one can submit data for diffing: 
 - _{host}/v1/diff/{ID}/left_
 - _{host}/v1/diff/{ID}/right_
 These support POST and PUT operations. The body is expected to be JSON document with single property `EncodedData`, which contains Base64 encoded bytes for diffing, e.g. `{ "EncodedData": "RGlmZmx5" }`
 
The third endpoint will return a diff result, in case both sides were already submitted to the server
 - GET _{host}/v1/diff/{ID}/_

----------

Sample Requests
-------------
Below curl requests will POST data for diffing to the left and right for id=1. Then will GET the result:

    curl -H "Content-Type: application/json" --data "{ \"EncodedData\": \"RGlmZmx5\" }"  http://localhost:22831/v1/diff/1/left
    
    curl -H "Content-Type: application/json" --data "{ \"EncodedData\": \"RGlmZmx5\" }"  http://localhost:22831/v1/diff/1/right
    
    curl -H "Content-Type: application/json" http://localhost:22831/v1/diff/1/

The last GET request will return `{"result":0,"diffs":[]}` since there is no diff.

Sample Responses
-------------
GET _{host}/v1/diff/{ID}/_ returns responses of format `{"result":0,"diffs":[]}` where the `result` is the enum value of [DiffResultType](https://github.com/theberserker/Waes.Diffly/blob/master/src/Waes.Diffly.Api.Dtos/Enums/DiffResultType.cs). `diffs` array contains indexes that differ in the provided byte arrays. It has value only in case `"result":2` (`DiffResultType.NotEqual`)

Client errors are returned with status 400-Bad Request with JSON document containing message property, e.g.  `{ "message": "Can not perform diff because no entry was provided for this id." }`


[Waes.Diffly.Api.Dtos](https://github.com/theberserker/Waes.Diffly/tree/master/src/Waes.Diffly.Api.Dtos) assembly contains all the DTOs that are required to work with the API in order to write a .NET client.

----------

Remarks and TODO
-------------
 - TODO: Caching
 - DELETE for _/left_ and _/right_ is not yet supported.