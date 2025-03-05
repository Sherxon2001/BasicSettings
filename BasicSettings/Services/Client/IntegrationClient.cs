using BasicSettings.Models.Additional;

namespace BasicSettings.Services.Client
{
    public class IntegrationClient
    {
        private readonly RestClient _client;
        private readonly ILoggerService _loggerService;

        public IntegrationClient(RestClient client, ILoggerService loggerService)
        {
            _client = client;
            _loggerService = loggerService;
        }

        public async Task<StateModel<TResponse>> SendAsync<TResponse>(
            string url,
            Method method = Method.Get,
            Tuple<object, string> body = null,
            List<Tuple<string, object, ParameterType>> parametr = null,
            CancellationToken cancellationToken = default)
        {
            var _state = StateModel<TResponse>.Create();
            var request = new RestRequest(url, method);

            if (body is not null)
            {
                request.AddHeader("Content-Type", body.Item2);
                request.AddJsonBody(body.Item1);
            }

            if (parametr?.Any() == true)
            {
                foreach (var item in parametr)
                    request.AddParameter(item.Item1, item.Item2, item.Item3);
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await _client.ExecuteAsync<TResponse>(request, cancellationToken);
                stopwatch.Stop();

                _loggerService.LogInformation(null, $"Request to {url} completed in {stopwatch.ElapsedMilliseconds} ms");

                _state.SetCode((int)response.StatusCode);
                _state.SetData(response.Data ?? default);

                if (!response.IsSuccessful)
                {
                    string errorMessage = !string.IsNullOrEmpty(response.Content) ? response.Content :
                                          !string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage :
                                          "Unexpected error occurred";
                    _state.SetMessage(errorMessage);
                }
            }
            catch (TaskCanceledException ex)
            {
                stopwatch.Stop();
                _loggerService.LogError(ex, $"Request to {url} timed out after {stopwatch.ElapsedMilliseconds} ms");

                _state.SetCode(StatusCodes.Status408RequestTimeout);
                _state.SetMessage("Request timed out: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                stopwatch.Stop();
                _loggerService.LogError(ex, $"Service unavailable for {url} after {stopwatch.ElapsedMilliseconds} ms");

                _state.SetCode(StatusCodes.Status503ServiceUnavailable);
                _state.SetMessage("Service unavailable: " + ex.Message);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _loggerService.LogError(ex, $"Request to {url} failed after {stopwatch.ElapsedMilliseconds} ms");

                _state.SetCode(StatusCodes.Status500InternalServerError);
                _state.SetMessage("Unhandled error: " + ex.Message);
            }

            return _state;
        }


        /*public async Task<StateModel<TResponse>> SendAsync<TResponse>(string url, Method method = Method.Get, Tuple<object, string> body = null, List<Tuple<string, object, ParameterType>> parametr = null, CancellationToken cancellationToken = default)
        {
            var _state = StateModel<TResponse>.Create();
            var request = new RestRequest(url, method);

            if (body is not null)
                request.AddBody(body.Item1, body.Item2);

            if (parametr is not null)
                foreach (var item in parametr)
                    request.AddParameter(item.Item1, item.Item2, item.Item3);

            //var stopwatch = Stopwatch.StartNew();
            try
            {
                //_loggerService.LogInformation(content: $"Sending request to {url} with method {method}");
                var response = await _client.ExecuteAsync<TResponse>(request, cancellationToken);
                //stopwatch.Stop();
                //_loggerService.LogInformation(content: $"Request to {url} completed in {stopwatch.ElapsedMilliseconds} ms");

                _state.SetCode((int)response.StatusCode);
                _state.SetData(response.Data);
                if (response.StatusCode != HttpStatusCode.OK)
                    _state.SetMessage(string.IsNullOrEmpty(response.Content) ? response.ErrorMessage : response.Content);

            }
            catch (Exception ex)
            {
                //stopwatch.Stop();
                //_loggerService.LogError(ex, content: $"Request to {url} failed after {stopwatch.ElapsedMilliseconds} ms");
                _state.SetCode(StatusCodes.Status400BadRequest);
                _state.SetMessage(ex.Message);
            }

            return _state;
        }*/
    }
}
