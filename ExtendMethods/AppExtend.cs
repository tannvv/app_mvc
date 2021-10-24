using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


namespace App.Extend {
    public static class AppExtend{
        public static void AddStatusCode(this IApplicationBuilder app){
            app.UseStatusCodePages(appError => {
                appError.Run(async context =>{
                    HttpResponse response = context.Response;
                    var code = response.StatusCode;
                    await response.WriteAsync("Error : " + (HttpStatusCode)code + "");
                });
            }); // code 400- 600
        }
    }
}