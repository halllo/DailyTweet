using System;
using System.Security.Cryptography;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TweetSharp;

namespace man.dailytweet
{
    public static class TimerTriggerCSharp
    {
        [FunctionName("TimerTriggerCSharp")]
        public static void Run([TimerTrigger("0 30 9 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function started at: {DateTime.Now}");

            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[100];
                rng.GetBytes(tokenData);
                var randomToken = Convert.ToBase64String(tokenData);

                var consumerKey = Environment.GetEnvironmentVariable("twitterconsumerkey");
                var consumerSecret = Environment.GetEnvironmentVariable("twitterconsumersecret");
                var accessToken = Environment.GetEnvironmentVariable("twitteraccesstoken");
                var accessTokenSecret = Environment.GetEnvironmentVariable("twitteraccesstokensecret");
                var service = new TwitterService(consumerKey, consumerSecret);
                service.AuthenticateWith(accessToken, accessTokenSecret);
                service.SendTweet(new SendTweetOptions() { Status = randomToken });
            }

            log.LogInformation($"C# Timer trigger function finished at: {DateTime.Now}");
        }
    }
}
