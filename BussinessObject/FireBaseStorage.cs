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
        private static string ApiKey = "AIzaSyDMPGlEuCrTZVTf6X5q46k9qX93OqpzY_c";
        private static string Bucket = "flower-shop-dfa19.appspot.com";
        //private static string AuthEmail = "kien@gmail.com";
        //private static string AuthPassword = "1234567890";
        public async Task<string> Upload(FileStream stream, string fileName)
        {

            string link = "";
            //var stream = new MemoryStream(Encoding.ASCII.GetBytes("Hello world!"));
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInAnonymouslyAsync();

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
                .Child("images")
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
