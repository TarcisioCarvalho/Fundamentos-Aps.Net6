using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            var result = new List<string>();
            foreach (var item in modelState.Values)
            {
                foreach (var erro in item.Errors)
                {
                    result.Add(erro.ErrorMessage);
                }
            }

            return result;
        }
    }
}