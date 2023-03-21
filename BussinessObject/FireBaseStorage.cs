using BusinessObject.IService;
using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class FireBaseStorage : IFireBaseStorage
    {
        private static string ApiKey = "AIzaSyD4xA9DbaOg7fjmwYj5nfd5ZUXavL41h4g";
        private static string Bucket = "flowershop-2e9af.appspot.com";
        public async Task<string> Upload(FileStream stream, string fileName)
        {

            string link = "";
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInAnonymouslyAsync();

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("product")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);

            try
            {
                return link = await task;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }
    }
}
