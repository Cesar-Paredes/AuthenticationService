using AuthenticationService.Models;
using AuthenticationService.ExceptionHandling;
using System.Text.Json;

namespace AuthenticationService.ExceptionHandling
{
    public class ResponseExceptionHandler
    {
      
        public string WrongPassword()
        {
            ExceptionModel responseModel = new()
            {
                Type = "https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-7.0",
                Title = "Bad Input",
                Status = 401,
                Description = "Password did not match!"
            };

            return JsonSerializer.Serialize<ExceptionModel>(responseModel);
        }

        public string UserNotFound()
        {
            ExceptionModel responseModel = new()
            {
                Type = "https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-7.0",
                Title = "Bad Input",
                Status = 400,
                Description = "The user is not found!"
            };

            return JsonSerializer.Serialize<ExceptionModel>(responseModel);
        }

        public string UserExist()
        {
            ExceptionModel responseModel = new()
            {
                Type = "https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-7.0",
                Title = "Bad Input",
                Status = 400,
                Description = "Choose another username. The username already exists! "
            };

            return JsonSerializer.Serialize<ExceptionModel>(responseModel);
        }


        public string IdDoesNotExist()
        {
            ExceptionModel responseModel = new()
            {
                Type = "https://dev.mysql.com/doc/refman/8.0/en/insert-on-duplicate.html",
                Title = "Bad Input",
                Status = 400,
                Description = "The ID entered does not exists, please enter a valid ID"
            };

            return JsonSerializer.Serialize<ExceptionModel>(responseModel);
        }

        //public string ValueDoesNotExist(string value)
        //{
        //    ExceptionModel responseModel = new()
        //    {
        //        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        //        Title = "Bad Input",
        //        Status = 400,
        //        Description = "The " + value + " entered does not exists, please enter a valid " + value
        //    };
        //    return JsonSerializer.Serialize<ExceptionModel>(responseModel);
        //}

        //public string EmptyList()
        //{
        //    ExceptionModel responseModel = new()
        //    {
        //        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        //        Title = "Empty Data List",
        //        Status = 409,
        //        Description = "The list requested is empty, please ensure the data was added sucessfully prior to running this service request"
        //    };
        //    return JsonSerializer.Serialize<ExceptionModel>(responseModel);
        //}
    }
}
