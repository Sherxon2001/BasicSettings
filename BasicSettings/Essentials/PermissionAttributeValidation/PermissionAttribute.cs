namespace BasicSettings.Essentials.PermissionAttributeValidation
{
    public class PermissionAttribute : TypeFilterAttribute
    {

        public PermissionAttribute() : base(typeof(AuthorizeAction)) { }
    }
}
