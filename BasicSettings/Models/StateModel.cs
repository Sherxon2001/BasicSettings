namespace BasicSettings.Models
{
    public class StateModel<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static StateModel<T> Create() => new StateModel<T>();

        public void SetCode(int code) => Code = code;
        public void SetMessage(string message) => Message = message;
        public void SetData(T data) => Data = data;
    }
}
