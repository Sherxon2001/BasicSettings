namespace BasicSettings.Models.Additional
{
    public class StateModel<T>
    {
        private int Code { get; set; }
        private string Message { get; set; }
        private T? Data { get; set; }

        public static StateModel<T> Create(int code = StatusCodes.Status400BadRequest, string message = "", T data = default)
        {
            return new StateModel<T>
            {
                Code = code,
                Message = message,
                Data = data
            };
        }

        public void SetCode(int code) => Code = code;
        public void SetMessage(string message) => Message = message;
        public void SetData(T data) => Data = data;
    }
}
